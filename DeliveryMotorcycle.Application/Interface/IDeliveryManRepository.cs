using DeliveryMotorcycle.Application.ViewModel;
using DeliveryMotorcycle.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Application.Interface
{
    public interface IDeliveryManRepository
    {
        ReturnViewModel CreateDeliveryMan(DeliveryManViewModel man, Guid userId);

        ReturnViewModel UpdateCnhDeliveryMan(string imagem, Guid userId);

        DeliveryMan? GetByCnpj(string cnpj);

        DeliveryMan? GetByCnh(string cnh);

        DeliveryMan? GetByUser(Guid userId);
    }
}
