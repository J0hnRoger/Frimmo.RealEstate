using System.Data;
using System.Threading;
using FluentAssertions;
using Xunit;

namespace Frimmo.RealEstateCalculator.Tests;

public class EstateSimulationTests
{
    [Fact]
    public void Simulation_WorstIt()
    {
        var property = new EstateProperty(250000, 190);
        property.SetMensualRentPrice(2000);
        var loan = new Loan(300000, 25, 0.012, 0.0034);
        // TODO - taxe foncière
        property.SetPropertyTax(700); 
        // TODO - travaux 
        property.SetRenovationWorks(50000);
        property.SetPropertyTax(1000); 
        property.SetCondominiumFeesByMonth(250);

        // TODO - amortissements
        var market = new Market("Les Herbiers", 2200, EstatePropertyType.Reno);
        var simulation = new EstateSimulation(property, loan, market);
        simulation.Evaluate().Should().Be(10);
        simulation.CashFlow.Should().Be(426);
    }

    [Fact]
    public void UnderMarketPrice_AppliedInEvaluation()
    {
        var property = new EstateProperty(243000, 190);
        property.SetMensualRentPrice(2200);
        var loan = new Loan(243000, 20, 0.032, 0.0034);
        // TODO - taxe foncière
        property.SetPropertyTax(1000); 
        // TODO - travaux 
        property.SetRenovationWorks(0);
        property.SetPropertyTax(1000); 
        property.SetCondominiumFeesByMonth(75);
        
        // TODO - amortissements
        var market = new Market("Mortagnes", 2000, EstatePropertyType.Reno); 
        var simulation = new EstateSimulation(property, loan, market);
        simulation.Evaluate().Should().Be(10);
        simulation.CashFlow.Should().Be(426);
    }
    
    [Fact]
    public void Check()
    {
        var property = new EstateProperty(160500, 73);
        property.SetMensualRentPrice(780);
        var loan = new Loan(160500, 25, 0.0343, 0.0034);
        // TODO - taxe foncière
        property.SetPropertyTax(700); 
        // TODO - travaux 
        property.SetRenovationWorks(40000);
        property.SetCondominiumFeesByMonth(50);
        
        // TODO - amortissements
        var market = new Market("Mortagnes", 2000, EstatePropertyType.Reno); 
        var simulation = new EstateSimulation(property, loan, market);
        simulation.Evaluate().Should().Be(10);
        simulation.CashFlow.Should().Be(426);
    }
}