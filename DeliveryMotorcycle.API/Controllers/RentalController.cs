using DeliveryMotorcycle.Application.Interface;
using DeliveryMotorcycle.Application.RabbitMqPublisher;
using DeliveryMotorcycle.Application.ViewModel;
using DeliveryMotorcycle.Domain.Entities;
using DeliveryMotorcycle.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;

namespace DeliveryMotorcycle.API.Controllers
{
    [ApiController]
    [Route("Rental")]
    [Authorize]
    public class RentalController : ControllerBase
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IPlanRepository _planRepository;
        private readonly ILogger<RentalController> _loggerError;
        private readonly UserManager<User> _userManager;

        public RentalController(ILogger<RentalController> loggerError, IRentalRepository rentalRepository, IPlanRepository planRepository, UserManager<User> userManager)
        {
            _loggerError = loggerError;
            _rentalRepository = rentalRepository;
            _planRepository = planRepository;
            _userManager = userManager;
        }

        [HttpPost("Create")]
        [SwaggerOperation(Summary = "Realizar a locação de uma moto")]
        public IActionResult Create([FromBody] RentalViewModel rental)
        {
            try
            {
                var response = _rentalRepository.CreateRental(rental, _userManager.GetUserAsync(User).Result.Id);

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

        [HttpGet("Plans")]
        [SwaggerOperation(Summary = "Consultar os planos das locações")]
        public IActionResult Plans()
        {
            try
            {
                var response = _planRepository.GetAll();

                return Ok(response);
            }
            catch (Exception e)
            {
                _loggerError.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Consultar locações por id")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var response = _rentalRepository.GetById(id);

                if (response == null)
                    return BadRequest("Rental not found!");

                return Ok(response);
            }
            catch (Exception e)
            {
                _loggerError.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}/Return")]
        [SwaggerOperation(Summary = "Informar a data de devolução e calcular o valor da locação")]
        public IActionResult Return(Guid id, [FromBody] RentalReturnViewModel rental)
        {
            try
            {
                var response = _rentalRepository.ReturnRental(rental, id);

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
