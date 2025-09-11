using DeliveryMotorcycle.API.Controllers;
using DeliveryMotorcycle.Application.ViewModel;
using DeliveryMotorcycle.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _config;
    private readonly ILogger<UserController> _loggerError;

    public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration config, ILogger<UserController> loggerError)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _config = config;
        _loggerError = loggerError;
    }

    [HttpPost("Register")]
    [SwaggerOperation(Summary = "Cadastrar um novo usuário")]
    public async Task<IActionResult> Register(string username, string password, string role = "User")
    {
        try
        {
            var user = new User { UserName = username, Role = role };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _userManager.AddToRoleAsync(user, role);

            return Ok("User registered!");
        }
        catch (Exception e)
        {
            _loggerError.LogError(e.Message);

            return BadRequest(e.Message);
        }
    }

    [HttpPost("Login")]
    [SwaggerOperation(Summary = "Realizar login do usuário")]
    public async Task<IActionResult> Login(string username, string password)
    {
        try
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
                return Unauthorized("User not found!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);

            if (!result.Succeeded)
                return Unauthorized("Invalid password!");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                role = user.Role
            });
        }
        catch (Exception e)
        {
            _loggerError.LogError(e.Message);

            return BadRequest(e.Message);
        }
    }
}
