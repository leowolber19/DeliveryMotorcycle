using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Domain.Entities
{
    public class DeliveryMan : BaseEntity
    {
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(14)]
        public string Cnpj { get; set; }

        public DateTime BirthDate { get; set; }

        [MaxLength(11)]
        public string CnhNumber { get; set; }

        public TypeCnh CnhType { get; set; }

        public string? CnhImageUrl { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public enum TypeCnh
        {
            A,
            B,
            AB
        }
    }
}
