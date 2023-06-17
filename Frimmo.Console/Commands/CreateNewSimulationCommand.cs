using Frimmo.RealEstateCalculator;
using Frimmo.RealEstateCalculator.Tests;
using Spectre.Console;

namespace Frimmo.Console.Commands;

public class CreateNewSimulationCommand
{
    int _priceInput;
    int _surface;
    int _rentPrice;
    int _renovationWorks;
    int _propertyTax;
    int _condominiumFees;

    public void ParseInput(string[] args)
    {
        // INPUT
        if (args.Length > 0 && args[0] == "--inline")
        {
            List<string> datas = args.Skip(1).ToList();
            _priceInput = int.Parse(datas[0]);
            _surface = int.Parse(datas[1]);
            _rentPrice = int.Parse(datas[2]);
            _renovationWorks = int.Parse(datas[3]);
            _propertyTax = int.Parse(datas[4]);
            _condominiumFees = int.Parse(datas[5]);
        }
        else
        {
            _priceInput = AnsiConsole.Ask<int>("Quel est le [green]prix FAI[/] du bien?");
            _surface = AnsiConsole.Ask<int>("Quel est la [green]Surface[/] du bien (pour le prix /m²)?");
            _rentPrice = AnsiConsole.Ask<int>("Quel est le [green] loyer attendu[/]?");
            _renovationWorks = AnsiConsole.Ask<int>("Quel est le [green] prix estimé des travaux[/]?");
            _propertyTax = AnsiConsole.Ask<int>("Quel est le [green] montant de la taxe foncière[/]?");
            _condominiumFees = AnsiConsole.Ask<int>("Quel est le [green] montant des charges estimées[/]?");
        }
    }

    public EstateSimulation Execute(Market currentMarket, int loanDuration = 20, double loanInterest = 0.032, double loanInsurance = 0.0034)
    {
        // ANALYSE
        var property = new EstateProperty(_priceInput, _surface);
        property.SetMensualRentPrice(_rentPrice);
        var loan = new Loan(_priceInput + _renovationWorks, loanDuration, loanInterest, loanInsurance);
        
        AnsiConsole.WriteLine($"Prêt: {loan.DurationInMonths / 12} ans - {loan.InterestRate}% d'interêt - {loan.InsuranceRate}% d'assurance");

        // TODO - taxe foncière
        property.SetPropertyTax(_propertyTax);
        // TODO - travaux 
        property.SetRenovationWorks(_renovationWorks);
        property.SetCondominiumFeesByMonth(_condominiumFees);

        var simulation = new EstateSimulation(property, loan, currentMarket);
        var result = simulation.Evaluate();
        
        DisplayResult(simulation, result);
        
        return simulation;
    }

    public static void DisplayResult(EstateSimulation simulation, EvaluationResult result)
    {
        // Create a table
        var table = new Table();

        // Add some columns
        table.AddColumn("Critère");
        table.AddColumn("OK/KO");
        table.AddColumn("Valeur actuelle");

        foreach (EvaluationRule rule in result.Rules)
        {
            table.AddRow(rule.Description,
                $"[#87005f]{(rule.IsValid ? "[green]OK[/]" : "[red]KO[/]")}[/]",
                rule.Explanation);
        }

        table.AddRow("Prix idéal", "", $"[green]{simulation.IdealPriceForPositiveCashflow}[/]€");

        // Render the table to the console
        AnsiConsole.Write(table);
    }
}