using DeliveryMotorcycle.Application.Interface;
using DeliveryMotorcycle.Domain.Entities;
using DeliveryMotorcycle.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Infrastructure.Repository
{
    public class MotorCycleNotificationRepository : IMotorCycleNotificationRepository
    {
        private readonly DeliveryMotorcycleDbContext _dbContext;

        public MotorCycleNotificationRepository(DeliveryMotorcycleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<MotorcycleNotification> GetAll()
        {
            return _dbContext.Notifications.Include(s => s.Motorcycle).OrderByDescending(o => o.CreatedAt).ToList();
        }
    }
}
