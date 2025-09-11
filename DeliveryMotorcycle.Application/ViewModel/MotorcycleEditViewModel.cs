using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Application.ViewModel
{
    public class MotorcycleEditViewModel
    {
        [Required]
        public string Plate { get; set; }
    }
}
