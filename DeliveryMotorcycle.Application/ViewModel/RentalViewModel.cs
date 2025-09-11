using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Application.ViewModel
{
    public class RentalViewModel
    {
        [Required]
        public int Days { get; set; }

        [Required]
        public DateTime ExpectedEndDate { get; set; }
    }
    public class RentalReturnViewModel
    {
        [Required]
        public DateTime ReturnDate { get; set; }
    }
}
