using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Application.ViewModel
{
    public class RentalValuesViewModel
    {
        public string Motorcycle { get; set; } 

        public string DeliveryMan { get; set; } 

        public int Plan { get; set; } 

        public double? RentalValue { get; set; } 

        public DateTime StartDate { get; set; } 

        public DateTime EndDate { get; set; } 

        public DateTime ExpectedEndDate { get; set; }

        public DateTime? ReturnDate { get; set; }
    }
}
