using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Domain.Entities
{
    public class Motorcycle : BaseEntity
    {
        public int Year { get; set; }

        [MaxLength(50)]
        public string Model { get; set; }

        [MaxLength(10)]
        public string Plate { get; set; }

        public StatusMotorcycle Status { get; set; }

        public enum StatusMotorcycle
        {
            Available,
            Rented
        }
    }
}
