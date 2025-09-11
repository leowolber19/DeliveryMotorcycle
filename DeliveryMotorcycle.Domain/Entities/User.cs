using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string Role { get; set; }
    }
}

