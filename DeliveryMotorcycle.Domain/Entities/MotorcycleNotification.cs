using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Domain.Entities
{
    public class MotorcycleNotification : BaseEntity
    {
        [ForeignKey("Motorcycle")]
        public Guid MotorcycleId { get; set; }
        public virtual Motorcycle Motorcycle { get; set; }
    }
}
