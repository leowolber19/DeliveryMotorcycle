using DeliveryMotorcycle.Application.Interface;
using DeliveryMotorcycle.Application.RabbitMqPublisher;
using DeliveryMotorcycle.Application.ViewModel;
using DeliveryMotorcycle.Domain.Entities;
using DeliveryMotorcycle.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Text;

namespace DeliveryMotorcycle.API.Controllers
{
    [ApiController]
    [Route("Notification")]
    [Authorize(Roles = "Admin")]
    public class NotificationsController : ControllerBase
    {
        private readonly IMotorCycleNotificationRepository _motorCycleNotificationRepository;
        private readonly ILogger<NotificationsController> _loggerError;

        public NotificationsController(IMotorCycleNotificationRepository motorCycleNotificationRepository, ILogger<NotificationsController> loggerError)
        {
            _motorCycleNotificationRepository = motorCycleNotificationRepository;
            _loggerError = loggerError;
        }

        [HttpGet("Get")]
        [SwaggerOperation(Summary = "Consultar todas as motos cadastradas com o ano de 2024")]
        public IActionResult Get()
        {
            try
            {
                var list = _motorCycleNotificationRepository.GetAll().Select(s => new MotorcycleNotificationViewModel()
                {
                    Model = s.Motorcycle.Model,
                    Plate = s.Motorcycle.Plate,
                    Year = s.Motorcycle.Year,
                    CreatedAt = s.Motorcycle.CreatedAt,
                });

                return Ok(list);
            }
            catch (Exception e)
            {
                _loggerError.LogError(e.Message);

                return BadRequest(e.Message);
            }
        }
    }
}
