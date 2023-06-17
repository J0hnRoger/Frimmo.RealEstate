using CSharpFunctionalExtensions;
using Frimmo.Console.Infrastructure;
using Frimmo.RealEstateCalculator;
using Spectre.Console;

namespace Frimmo.Console.Commands;

public class DeleteExistingSimulationCommand
{
    private readonly EstateSimulationRepository _simulationRepository;
    private EstateSimulation _currentSimulation;
    
    public DeleteExistingSimulationCommand(EstateSimulationRepository simulationRepository)
    {
        _simulationRepository = simulationRepository;
    }
    
    public void ParseInput()
    {
        // INPUT
        string simulationName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Selectionner une simulation enregistrée ?")
            .AddChoiceGroup("Simulations:", _simulationRepository.AllEstateSimulations
                .Select(m => m.ToString())));

        _currentSimulation = _simulationRepository.AllEstateSimulations
            .FirstOrDefault(m => m.ToString() == simulationName);
    }

    public void Execute()
    {
        _simulationRepository.AllEstateSimulations.Remove(_currentSimulation);
        _simulationRepository.Save();
        AnsiConsole.WriteLine($"Simulation supprimeé: {_currentSimulation}");
    }
}