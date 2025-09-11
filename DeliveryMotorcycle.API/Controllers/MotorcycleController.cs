using DeliveryMotorcycle.Application.Interface;
using DeliveryMotorcycle.Application.RabbitMqPublisher;
using DeliveryMotorcycle.Application.ViewModel;
using DeliveryMotorcycle.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Annotations;
using System.Text;

namespace DeliveryMotorcycle.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class MotorcyclesController : ControllerBase
    {
        private readonly IMotorcycleRepository _motorCycleRepository;
        private readonly ILogger<MotorcyclesController> _loggerError;

        public MotorcyclesController(IMotorcycleRepository motorCycleRepository, ILogger<MotorcyclesController> loggerError)
        {
            _motorCycleRepository = motorCycleRepository;
            _loggerError = loggerError;
        }

        [HttpPost("Create")]
        [SwaggerOperation(Summary = "Cadastrar uma nova moto")]
        public IActionResult Create([FromBody] MotorcycleViewModel motorcycle)
        {
            try
            {
                var response = _motorCycleRepository.CreateMotorcycle(motorcycle);

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

        [HttpGet("GetByPlate")]
        [SwaggerOperation(Summary = "Consultar uma moto filtrando pela placa")]
        public IActionResult GetByPlate(string plate)
        {
            try
            {
                var response = _motorCycleRepository.GetMotorcycleByPlate(plate);

                if (response == null)
                    return BadRequest("Motorclycle not found!");

                return Ok(response);
            }
            catch (Exception e)
            {
                _loggerError.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }

        [HttpPut("Edit/{id}/Plate")]
        [SwaggerOperation(Summary = "Alterar a placa de uma moto")]
        public IActionResult EditMotorcycleByPlate(Guid id, [FromBody] MotorcycleEditViewModel request)
        {
            try
            {
                var response = _motorCycleRepository.EditMotorcycleByPlate(id, request.Plate);

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

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Consultar motos existentes por id")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var response = _motorCycleRepository.GetById(id);

                if (response == null)
                    return BadRequest("Motorclycle not found!");

                return Ok(response);
            }
            catch (Exception e)
            {
                _loggerError.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        [SwaggerOperation(Summary = "Excluir motos existentes por id")]
        public IActionResult DeleteById(Guid id)
        {
            try
            {
                var response = _motorCycleRepository.DeleteById(id);

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
