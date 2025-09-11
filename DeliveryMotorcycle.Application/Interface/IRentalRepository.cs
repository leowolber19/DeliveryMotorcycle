using DeliveryMotorcycle.Application.ViewModel;
using DeliveryMotorcycle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Application.Interface
{
    public interface IRentalRepository
    {
        ReturnViewModel CreateRental(RentalViewModel rental, Guid userId);

        ReturnViewModel ReturnRental(RentalReturnViewModel rental, Guid rentalId);

        RentalValuesViewModel? GetById(Guid id);
    }
}
