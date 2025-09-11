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
    }
}
