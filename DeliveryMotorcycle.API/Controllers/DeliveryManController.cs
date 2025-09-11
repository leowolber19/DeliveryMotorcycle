using DeliveryMotorcycle.Application.Interface;
using DeliveryMotorcycle.Application.ViewModel;
using DeliveryMotorcycle.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DeliveryMotorcycle.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class DeliveryManController : ControllerBase
    {
        private readonly IDeliveryManRepository _deliveryManRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DeliveryManController> _loggerError;

        public DeliveryManController(IDeliveryManRepository deliveryManRepository, UserManager<User> userManager, ILogger<DeliveryManController> loggerError)
        {
            _deliveryManRepository = deliveryManRepository;
            _userManager = userManager;
            _loggerError = loggerError;
        }

        [HttpPost("Create")]
        [SwaggerOperation(Summary = "Cadastrar um novo entregador")]
        public IActionResult Create([FromBody] DeliveryManViewModel man)
        {
            try
            {
                var response = _deliveryManRepository.CreateDeliveryMan(man, _userManager.GetUserAsync(User).Result.Id);

                if (!response.Success)
                    return BadRequest(response);

                return Ok(response);
            }
            catch (Exception e)
            {
                _loggerError.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }

        [HttpPut("UpdateCnh")]
        [SwaggerOperation(Summary = "Atualizar a cnh do entregador")]
        public IActionResult UpdateCnh(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("Invalid file.");

                var extension = Path.GetExtension(file.FileName).ToLower();

                if (extension != ".png" && extension != ".bmp")
                    return BadRequest("Only PNG or BMP files are allowed.");

                var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "cnh");

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var response = _deliveryManRepository.UpdateCnhDeliveryMan("/uploads/cnh/" + fileName, _userManager.GetUserAsync(User).Result.Id);

                if (!response.Success)
                    return BadRequest(response);

                return Ok(response);
            }
            catch (Exception e)
            {
                _loggerError.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }
    }
}
