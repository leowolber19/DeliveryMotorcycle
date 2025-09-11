using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Application.ViewModel
{
    public class MotocycleConsumerViewModel
    {
        public Guid Id { get; set; }

        public int Year { get; set; }

        public string Model { get; set; }

        public string Plate { get; set; }
    }
}
