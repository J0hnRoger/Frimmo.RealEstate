using Frimmo.Console.Infrastructure;
using Frimmo.RealEstateCalculator;
using Spectre.Console;

namespace Frimmo.Console.Commands;

public class SaveSimulationCommand
{
    private readonly EstateSimulationRepository _repository;

    public SaveSimulationCommand(EstateSimulationRepository repository)
    {
        _repository = repository;
    }

    public void Execute(EstateSimulation savingSimulation)
    {
        var save = AnsiConsole.Confirm("Voulez-vous sauvegarder la simulation?");
        if (!save)
            return;
        string description = AnsiConsole.Ask<string>("Donnez une description à cette simulation pour la retrouver plus tard:");
        savingSimulation.Property.Description = description;
        
        _repository.AllEstateSimulations.Add(savingSimulation);
        _repository.Save();
        
        AnsiConsole.Write("Simulation sauvegardée");
    }
}