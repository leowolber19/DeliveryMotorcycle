using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Application.ViewModel
{
    public class MotorcycleViewModel
    {
        [Required]
        public int Year { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public string Plate { get; set; }
    }
}
