using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Application.ViewModel
{
    public class MotorcycleNotificationViewModel
    {
        public string Model { get; set; }
        public string Plate { get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
