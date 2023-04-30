using Frimmo.Console.Infrastructure;
using Frimmo.RealEstateCalculator;
using Spectre.Console;

namespace Frimmo.Console.Commands;

public class EditExistingSimulationCommand
{
    private readonly EstateSimulationRepository _simulationRepository;
    private EstateSimulation _currentSimulation;
    
    public EditExistingSimulationCommand(EstateSimulationRepository simulationRepository)
    {
        _simulationRepository = simulationRepository;
    }
    
    public void ParseInput(string[] args)
    {
        // INPUT
        string simulationName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Selectionner un marché ?")
            .AddChoiceGroup("Marchés disponibles:", _simulationRepository.AllEstateSimulations
                .Select(m => m.ToString())));

        _currentSimulation = _simulationRepository.AllEstateSimulations
            .First(m => m.ToString() == simulationName);
    }
}