using DeliveryMotorcycle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Application.Interface
{
    public interface IMotorCycleNotificationRepository
    {
        List<MotorcycleNotification> GetAll();
    }
}
