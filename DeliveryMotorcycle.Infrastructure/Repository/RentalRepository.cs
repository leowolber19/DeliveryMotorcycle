using DeliveryMotorcycle.Application.Interface;
using DeliveryMotorcycle.Application.RabbitMqPublisher;
using DeliveryMotorcycle.Application.ViewModel;
using DeliveryMotorcycle.Domain.Entities;
using DeliveryMotorcycle.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryMotorcycle.Infrastructure.Repository
{
    public class RentalRepository : IRentalRepository
    {
        private readonly DeliveryMotorcycleDbContext _dbContext;
        private readonly IDeliveryManRepository _deliveryManRepository;
        private readonly IPlanRepository _planRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;

        public RentalRepository(DeliveryMotorcycleDbContext dbContext, IDeliveryManRepository deliveryManRepository, IPlanRepository planRepository, IMotorcycleRepository motorcycleRepository)
        {
            _dbContext = dbContext;
            _deliveryManRepository = deliveryManRepository;
            _planRepository = planRepository;
            _motorcycleRepository = motorcycleRepository;
        }

        public ReturnViewModel CreateRental(RentalViewModel rental, Guid userId)
        {
            try
            {
                var deliveryMan = _deliveryManRepository.GetByUser(userId);

                if (deliveryMan == null)
                    return new ReturnViewModel(false, "Error to create rental: delivery man not found!");

                if (deliveryMan.CnhType != DeliveryMan.TypeCnh.A && deliveryMan.CnhType != DeliveryMan.TypeCnh.AB)
                    return new ReturnViewModel(false, "Error to create rental: the delivery man must have type A license!");

                var motorcycle = _motorcycleRepository.GetMotorcycleAvailable();

                if (motorcycle == null)
                    return new ReturnViewModel(false, "Error to create rental: no motorcycle available!");

                var plan = _planRepository.GetByDays(rental.Days);

                if (plan == null)
                    return new ReturnViewModel(false, "Error to create rental: plan not found!");

                var rent = new Rental()
                {
                    MotorcycleId = motorcycle.Id,
                    DeliveryManId = deliveryMan.Id,
                    MotorcyclePlanId = plan.Id,
                    StartDate = DateTime.UtcNow.AddDays(1),
                    EndDate = DateTime.UtcNow.AddDays(rental.Days),
                    ExpectedEndDate = rental.ExpectedEndDate,
                };

                _dbContext.Rentals.Add(rent);

                motorcycle.Status = Motorcycle.StatusMotorcycle.Rented;

                _dbContext.Motorcycle.Update(motorcycle);

                _dbContext.SaveChanges();

                return new ReturnViewModel(true, "Success to create rental! RentalID: " + rent.Id);
            }
            catch (Exception e)
            {
                return new ReturnViewModel(false, "Error to create rental: " + e.Message);
            }
        }

        public RentalValuesViewModel? GetById(Guid id)
        {
            var rental = _dbContext.Rentals
                                .Include(i => i.Motorcycle)
                                    .Include(i => i.DeliveryMan)
                                        .Include(i => i.MotorcyclePlan)
                                            .FirstOrDefault(f => f.Id == id);

            if (rental == null) return null;

            return new RentalValuesViewModel()
            {
                Motorcycle = rental.Motorcycle.Model,
                DeliveryMan = rental.DeliveryMan.Name,
                Plan = rental.MotorcyclePlan.Days,
                RentalValue = rental.Value,
                StartDate = rental.StartDate,
                EndDate = rental.EndDate,
                ExpectedEndDate = rental.ExpectedEndDate,
                ReturnDate = rental.ReturnDate,
            };
        }

        public ReturnViewModel ReturnRental(RentalReturnViewModel rental, Guid rentalId)
        {
            try
            {
                var rent = _dbContext.Rentals.Include(i => i.MotorcyclePlan).FirstOrDefault(f => f.Id == rentalId);

                if (rent == null)
                    return new ReturnViewModel(false, "Error saving return date: rental not found!");

                rent.ReturnDate = rental.ReturnDate;

                var totalValue = CalculeValueTotal(rent);

                rent.Value = (double)totalValue;

                _dbContext.Rentals.Update(rent);
                _dbContext.SaveChanges();

                return new ReturnViewModel(true, "Success saving return date! Value rental: " + totalValue.ToString("C", new CultureInfo("pt-BR")));
            }
            catch (Exception e)
            {
                return new ReturnViewModel(false, "Error saving return date: " + e.Message);
            }
        }

        public decimal CalculeValueTotal(Rental rent)
        {
            try
            {
                decimal totalValue = 0m;

                int daysUsed = (rent.ReturnDate.Value.Date - rent.StartDate.Date.AddDays(-1)).Days;

                if (daysUsed <= 0) daysUsed = 1;

                var days = rent.MotorcyclePlan.Days;
                var dailyValue = rent.MotorcyclePlan.Value;

                if (rent.ReturnDate < rent.ExpectedEndDate)
                {
                    totalValue = daysUsed * dailyValue;

                    var unusedDays = days - daysUsed;

                    if (unusedDays > 0)
                    {
                        decimal multaPercent = 0m;

                        if (days == 7)
                        {
                            multaPercent = 0.20m;
                        }
                        else if (days == 15)
                        {
                            multaPercent = 0.40m;
                        }

                        totalValue += unusedDays * dailyValue * multaPercent;
                    }
                }
                else if (rent.ReturnDate > rent.ExpectedEndDate)
                {
                    totalValue = days * dailyValue;

                    var extraDays = (rent.ReturnDate.Value.Date - rent.ExpectedEndDate.Date).Days;

                    if (extraDays > 0)
                        totalValue += extraDays * 50;
                }
                else
                {
                    totalValue = days * dailyValue;
                }

                return totalValue;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
