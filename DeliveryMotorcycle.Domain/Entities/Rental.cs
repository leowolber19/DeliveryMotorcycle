using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Domain.Entities
{
    public class Rental :  BaseEntity
    {
        [ForeignKey("Motorcycle")]
        public Guid MotorcycleId { get; set; }
        public virtual Motorcycle Motorcycle { get; set; }

        [ForeignKey("DeliveryMan")]
        public Guid DeliveryManId { get; set; }
        public virtual DeliveryMan DeliveryMan { get; set; }

        [ForeignKey("MotorcyclePlan")]
        public Guid MotorcyclePlanId { get; set; }
        public virtual Plan MotorcyclePlan { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime ExpectedEndDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public double? Value { get; set; }

        public decimal CalcularMulta(DateTime returnDate)
        {
            if (returnDate < ExpectedEndDate)
            {
                var diasNaoUsados = (ExpectedEndDate - returnDate).Days;
                var percentual = (decimal)(MotorcyclePlan.Percentage ?? 0);

                return diasNaoUsados * MotorcyclePlan.Value * percentual;
            }
            else if (returnDate > ExpectedEndDate)
            {
                var diasExtras = (returnDate - ExpectedEndDate).Days;
                return diasExtras * 50m;
            }

            return 0m;
        }
    }
}
