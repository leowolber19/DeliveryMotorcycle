using DeliveryMotorcycle.Application.Interface;
using DeliveryMotorcycle.Application.RabbitMqPublisher;
using DeliveryMotorcycle.Application.ViewModel;
using DeliveryMotorcycle.Domain.Entities;
using DeliveryMotorcycle.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Infrastructure.Repository
{
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly DeliveryMotorcycleDbContext _dbContext;
        private readonly IRabbitMqPublisher _publisher;

        public MotorcycleRepository(DeliveryMotorcycleDbContext dbContext, IRabbitMqPublisher publisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
        }

        public ReturnViewModel CreateMotorcycle(MotorcycleViewModel motorcycle)
        {
            try
            {
                if (GetMotorcycleByPlate(motorcycle.Plate) != null)
                    return new ReturnViewModel(false, "Error to create motorcycle: Existing plate");

                var motorcycleObj = new Motorcycle()
                {
                    Year = motorcycle.Year,
                    Model = motorcycle.Model,
                    Plate = motorcycle.Plate
                };

                _dbContext.Motorcycle.Add(motorcycleObj);
                _dbContext.SaveChanges();

                _publisher.Publish(motorcycleObj, "motorcycle_queue");

                return new ReturnViewModel(true, "Success to create motorcycle!");
            }
            catch (Exception e)
            {
                return new ReturnViewModel(false, "Error to create motorcycle: " + e.Message);
            }
        }

        public Motorcycle? GetMotorcycleByPlate(string plate)
        {
            return _dbContext.Motorcycle.FirstOrDefault(f => f.Plate.ToUpper() == plate.ToUpper() && !f.Excluded);
        }

        public Motorcycle? GetMotorcycleAvailable()
        {
            return _dbContext.Motorcycle.FirstOrDefault(f => !f.Excluded && f.Status == Motorcycle.StatusMotorcycle.Available);
        }

        public ReturnViewModel EditMotorcycleByPlate(Guid id, string plate)
        {
            try
            {
                if (GetMotorcycleByPlate(plate) != null)
                    return new ReturnViewModel(false, "Error to edit motorcycle: Existing plate");

                var motorcycle = GetById(id);

                if (motorcycle == null)
                    return new ReturnViewModel(false, "Motorcycle not found!");

                motorcycle.Plate = plate;

                _dbContext.Motorcycle.Update(motorcycle);
                _dbContext.SaveChanges();

                return new ReturnViewModel(true, "Success to edit motorcycle!");
            }
            catch (Exception e)
            {
                return new ReturnViewModel(false, "Error to edit motorcycle: " + e.Message);
            }
        }

        public Motorcycle? GetById(Guid id)
        {
            return _dbContext.Motorcycle.FirstOrDefault(f => f.Id == id);
        }

        public ReturnViewModel DeleteById(Guid id)
        {
            try
            {
                var motorcycle = GetById(id);

                if (motorcycle == null)
                    return new ReturnViewModel(false, "Motorcycle not found!");

                if (motorcycle.Excluded)
                    return new ReturnViewModel(false, "Motorcycle is already excluded!");

                if (motorcycle.Status == Motorcycle.StatusMotorcycle.Rented)
                    return new ReturnViewModel(false, "The motorcycle is for rent!");

                motorcycle.Excluded = true;
                motorcycle.DeleteAt = DateTime.UtcNow;

                _dbContext.Motorcycle.Update(motorcycle);
                _dbContext.SaveChanges();

                return new ReturnViewModel(true, "Success to delete motorcycle!");
            }
            catch (Exception e)
            {
                return new ReturnViewModel(false, "Error to delete motorcycle: " + e.Message);
            }
        }
    }
}
