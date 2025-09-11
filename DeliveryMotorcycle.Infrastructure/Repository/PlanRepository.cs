using DeliveryMotorcycle.Application.Interface;
using DeliveryMotorcycle.Domain.Entities;
using DeliveryMotorcycle.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Infrastructure.Repository
{
    public class PlanRepository : IPlanRepository
    {
        private readonly DeliveryMotorcycleDbContext _dbContext;

        public PlanRepository(DeliveryMotorcycleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Plan> GetAll()
        {
            return _dbContext.Plans.OrderBy(f => f.Days).ToList();
        }

        public Plan? GetByDays(int days)
        {
            return _dbContext.Plans.FirstOrDefault(f => f.Days == days);
        }
    }
}
