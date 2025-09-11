using DeliveryMotorcycle.API.RabbitMqConsumer;
using DeliveryMotorcycle.Application.Interface;
using DeliveryMotorcycle.Application.RabbitMqPublisher;
using DeliveryMotorcycle.Domain.Entities;
using DeliveryMotorcycle.Infrastructure.Data;
using DeliveryMotorcycle.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<DeliveryMotorcycleDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// RabbitMQ Client
builder.Services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
{
    HostName = builder.Configuration["RabbitMq:Host"],
    UserName = builder.Configuration["RabbitMq:Username"],
    Password = builder.Configuration["RabbitMq:Password"]
});

builder.Services.AddHostedService<MotorcycleConsumer>();

// Singleton para manter a conexão aberta
builder.Services.AddSingleton(sp =>
{
    var factory = sp.GetRequiredService<IConnectionFactory>();
    return factory.CreateConnection();
});

// Singleton para manter o canal aberto
builder.Services.AddSingleton(sp =>
{
    var connection = sp.GetRequiredService<IConnection>();
    var channel = connection.CreateModel();
    channel.QueueDeclare(queue: "motorcycle_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
    return channel;
});

// Repository
builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IMotorCycleNotificationRepository, MotorCycleNotificationRepository>();
builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqPublisher>();
builder.Services.AddScoped<IDeliveryManRepository, DeliveryManRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<DeliveryMotorcycleDbContext>()
    .AddDefaultTokenProviders();

// JWT
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddSwaggerGen(c =>
{
    // Info básica
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Delivery Motorcycle API",
        Version = "v1"
    });

    // Define o esquema de segurança JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token}"
    });

    // Aplica a segurança globalmente
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });

    c.EnableAnnotations();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DeliveryMotorcycleDbContext>();

    context.Database.Migrate();

    await DeliveryMotorcycle.Domain.Base.Roles.Initialize(services);
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DeliveryMotorcycleDbContext>();

    if (!context.Plans.Any())
    {
        context.Plans.AddRange(
            new Plan { Days = 7, Value = 30, Percentage = 0.2 },
            new Plan { Days = 15, Value = 28, Percentage = 0.4 },
            new Plan { Days = 30, Value = 22 },
            new Plan { Days = 45, Value = 20 },
            new Plan { Days = 50, Value = 18 }
        );

        context.SaveChanges();
    }
}

app.UseStaticFiles();

// Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();

app.Use(async (context, next) =>
{
    await next();

    if (context.Response.StatusCode == 403)
    {
        context.Response.ContentType = "application/json";

        var result = System.Text.Json.JsonSerializer.Serialize(new
        {
            message = "Access denied! You do not have permission for this method."
        });

        await context.Response.WriteAsync(result);
    }
});

app.UseAuthorization();
app.MapControllers();
app.Run();
