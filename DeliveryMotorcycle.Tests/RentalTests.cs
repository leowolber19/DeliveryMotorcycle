using Xunit;
using DeliveryMotorcycle.Domain.Entities;
using System;

public class RentalTests
{
    [Fact]
    public void CalcularMultaAntesPrazo()
    {
        var plano = new Plan { Days = 7, Value = 30m, Percentage = 0.20 };
        var rental = new Rental
        {
            StartDate = DateTime.Today,
            ExpectedEndDate = DateTime.Today.AddDays(7),
            EndDate = DateTime.Today.AddDays(7),
            MotorcyclePlan = plano
        };

        var returnDate = DateTime.Today.AddDays(5);

        var diasNaoUsados = (rental.ExpectedEndDate - returnDate).Days;
        var multaEsperada = diasNaoUsados * plano.Value * 0.20m;

        var multa = rental.CalcularMulta(returnDate);

        Assert.Equal(multaEsperada, multa);
    }

    [Fact]
    public void CalcularMultaAposPrazo()
    {
        var plano = new Plan { Days = 7, Value = 30m, Percentage = 0.20 };
        var rental = new Rental
        {
            StartDate = DateTime.Today,
            ExpectedEndDate = DateTime.Today.AddDays(7),
            EndDate = DateTime.Today.AddDays(7),
            MotorcyclePlan = plano
        };

        var returnDate = DateTime.Today.AddDays(9);
        var diasExtras = (returnDate - rental.ExpectedEndDate).Days;
        var multaEsperada = diasExtras * 50m;

        var multa = rental.CalcularMulta(returnDate);

        Assert.Equal(multaEsperada, multa);
    }

    [Fact]
    public void CalcularSemMulta()
    {
        var plano = new Plan { Days = 7, Value = 30m, Percentage = 0.20 };
        var rental = new Rental
        {
            StartDate = DateTime.Today,
            ExpectedEndDate = DateTime.Today.AddDays(7),
            EndDate = DateTime.Today.AddDays(7),
            MotorcyclePlan = plano
        };

        var returnDate = rental.ExpectedEndDate;
        var multaEsperada = 0m;

        var multa = rental.CalcularMulta(returnDate);

        Assert.Equal(multaEsperada, multa);
    }
}
