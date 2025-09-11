using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DeliveryMotorcycle.Domain.Entities.DeliveryMan;

namespace DeliveryMotorcycle.Application.ViewModel
{
    public class DeliveryManViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Cnpj { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public string CnhNumber { get; set; }

        [Required]
        public TypeCnh CnhType { get; set; }
    }
}
