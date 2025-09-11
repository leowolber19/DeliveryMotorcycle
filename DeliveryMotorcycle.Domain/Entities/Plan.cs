using Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Domain.Entities
{
    public class Plan : BaseEntity
    {
        public int Days { get; set; }

        public decimal Value { get; set; }

        public double? Percentage { get; set; }

        public static Plan CreateMotorcyclePlan(int days, decimal value, double? percentage = null)
        {
            var motorcyclePlan = new Plan()
            {
                Days = days,
                Value = value,
                Percentage = percentage,
                Id = Guid.NewGuid(),
                CreatedAt = DateTime.UtcNow
            };

            return motorcyclePlan;
        }
    }
}
