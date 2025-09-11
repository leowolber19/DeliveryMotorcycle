using DeliveryMotorcycle.Application.ViewModel;
using DeliveryMotorcycle.Domain.Entities;
using DeliveryMotorcycle.Infrastructure.Data;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DeliveryMotorcycle.API.RabbitMqConsumer
{
    public class MotorcycleConsumer : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IServiceScopeFactory _scopeFactory;

        public MotorcycleConsumer(IModel channel, IServiceScopeFactory scopeFactory)
        {
            _channel = channel;
            _scopeFactory = scopeFactory;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                var motorcycle = JsonSerializer.Deserialize<MotocycleConsumerViewModel>(message);

                if (motorcycle != null && motorcycle.Year == 2024)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<DeliveryMotorcycleDbContext>();

                    var notification = new MotorcycleNotification
                    {
                        MotorcycleId = motorcycle.Id,
                        Excluded = false
                    };

                    db.Notifications.Add(notification);

                    await db.SaveChangesAsync(stoppingToken);
                }
            };

            _channel.BasicConsume(queue: "motorcycle_queue", autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
