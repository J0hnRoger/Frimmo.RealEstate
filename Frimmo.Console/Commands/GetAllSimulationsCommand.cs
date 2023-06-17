using Frimmo.Console.Infrastructure;
using Frimmo.RealEstateCalculator;
using Spectre.Console;

namespace Frimmo.Console.Commands;

public class GetAllSimulationsCommand
{
    private readonly EstateSimulationRepository _simulationRepository;

    public GetAllSimulationsCommand(EstateSimulationRepository simulationRepository)
    {
        _simulationRepository = simulationRepository;
    }

    public void Execute()
    {
        var orderedSimulations = _simulationRepository.AllEstateSimulations
            .OrderByDescending(s => s.CashFlow);

        DisplaySimulations(orderedSimulations.ToList());
        
        AnsiConsole.WriteLine($"");
    }

    public void DisplaySimulations(List<EstateSimulation> orderedSimulations)
    {
        // Create a table
        var table = new Table();

        // Add some columns
        table.AddColumn("ID");
        table.AddColumn("Description");
        table.AddColumn("Prix");
        table.AddColumn("Created");
        table.AddColumn("Cashflow");
        table.AddColumn("Prêt");
        table.AddColumn("Prix ideal");

        foreach (EstateSimulation simulation in orderedSimulations)
        {
            table.AddRow(simulation.Id.ToString(), simulation.Property.Description,
                simulation.Property.FullPrice +  $" euros (prix: {simulation.Property.Price} + frais de notaires: {simulation.Property.NotariesFees} + travaux: {simulation.Property.RenovationWork} euros)", "Date",
                $"{simulation.CashFlow} euros", $"{simulation.Loan}", $"{simulation.IdealPriceForPositiveCashflow} euros");
        }

        // Render the table to the console
        AnsiConsole.Write(table);
        int simulationId = AnsiConsole.Ask<int>("Voir les détails de l'annonce n°?");

        EstateSimulation selectedSimulation = _simulationRepository.Get(simulationId);        
        // TODO - internaliser le .Evaluate
        CreateNewSimulationCommand.DisplayResult(selectedSimulation, selectedSimulation.Evaluate()); 
    }
}