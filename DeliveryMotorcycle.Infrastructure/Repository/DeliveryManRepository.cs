using DeliveryMotorcycle.Application.Interface;
using DeliveryMotorcycle.Application.RabbitMqPublisher;
using DeliveryMotorcycle.Application.ViewModel;
using DeliveryMotorcycle.Domain.Entities;
using DeliveryMotorcycle.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Infrastructure.Repository
{
    public class DeliveryManRepository : IDeliveryManRepository
    {
        private readonly DeliveryMotorcycleDbContext _dbContext;

        public DeliveryManRepository(DeliveryMotorcycleDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ReturnViewModel CreateDeliveryMan(DeliveryManViewModel man, Guid userId)
        {
            try
            {
                if (GetByUser(userId) != null)
                    return new ReturnViewModel(false, "Error to create delivery man: There is already a delivery person registered for this user!");

                if (GetByCnh(man.CnhNumber) != null)
                    return new ReturnViewModel(false, "Error to create delivery man: driver's license already exists registered!");

                if (GetByCnpj(man.Cnpj) != null)
                    return new ReturnViewModel(false, "Error to create delivery man: cnpj already exists registered!");

                if (Regex.Replace(man.Cnpj, @"\D", "").Length > 14)
                    return new ReturnViewModel(false, "Error to create delivery man: cnpj invalid!");

                if (Regex.Replace(man.CnhNumber, @"\D", "").Length > 11)
                    return new ReturnViewModel(false, "Error to create delivery man: driver's license invalid!");

                var deliveryMan = new DeliveryMan()
                {
                    Name = man.Name,
                    Cnpj = Regex.Replace(man.Cnpj, @"\D", ""),
                    BirthDate = man.BirthDate,
                    CnhNumber = Regex.Replace(man.CnhNumber, @"\D", ""),
                    CnhType = man.CnhType,
                    UserId = userId
                };

                _dbContext.DeliveryMans.Add(deliveryMan);
                _dbContext.SaveChanges();

                return new ReturnViewModel(true, "Success to create delivery man!");
            }
            catch (Exception e)
            {
                return new ReturnViewModel(false, "Error to create delivery man: " + e.Message + ". " + e?.InnerException?.Message);
            }
        }

        public DeliveryMan? GetByCnh(string cnh)
        {
            return _dbContext.DeliveryMans.FirstOrDefault(f => f.CnhNumber == Regex.Replace(cnh, @"\D", "") && !f.Excluded);
        }

        public DeliveryMan? GetByCnpj(string cnpj)
        {
            return _dbContext.DeliveryMans.FirstOrDefault(f => f.Cnpj == Regex.Replace(cnpj, @"\D", "") && !f.Excluded);
        }

        public DeliveryMan? GetByUser(Guid userId)
        {
            return _dbContext.DeliveryMans.FirstOrDefault(f => f.UserId == userId && !f.Excluded);
        }

        public ReturnViewModel UpdateCnhDeliveryMan(string imagem, Guid userId)
        {
            try
            {
                var deliveryMan = GetByUser(userId);

                if (deliveryMan == null)
                    return new ReturnViewModel(false, "There is still no delivery person for this user!");

                deliveryMan.CnhImageUrl = imagem;

                _dbContext.DeliveryMans.Update(deliveryMan);
                _dbContext.SaveChanges();

                return new ReturnViewModel(true, "Success updating driver's license!");
            }
            catch (Exception e)
            {
                return new ReturnViewModel(false, "Error updating driver's license: " + e.Message);
            }
        }
    }
}
