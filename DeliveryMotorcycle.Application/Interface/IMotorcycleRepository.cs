using DeliveryMotorcycle.Application.ViewModel;
using DeliveryMotorcycle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Application.Interface
{
    public interface IMotorcycleRepository
    {
        ReturnViewModel CreateMotorcycle(MotorcycleViewModel motorcycle);

        Motorcycle? GetMotorcycleByPlate(string plate);

        Motorcycle? GetMotorcycleAvailable();

        ReturnViewModel EditMotorcycleByPlate(Guid id, string plate);

        Motorcycle? GetById(Guid id);

        ReturnViewModel DeleteById(Guid id);
    }
}
