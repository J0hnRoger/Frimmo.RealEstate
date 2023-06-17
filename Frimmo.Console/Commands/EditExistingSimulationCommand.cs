using CSharpFunctionalExtensions;
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
        ParseUpdateValues();
    }

    private void ParseUpdateValues()
    {
        _currentSimulation.Property.Price =  
            AnsiConsole.Ask<int>($"Quel est le [green]prix FAI[/] du bien?({_currentSimulation.Property.Price})");

        _currentSimulation.Property.Surface = AnsiConsole.Ask<int>(
            $"Quel est la [green]Surface[/] du bien (pour le prix /m²)?({_currentSimulation.Property.Surface})");

        _currentSimulation.Property.MensualRent =
            AnsiConsole.Ask<int>($"Quel est le [green] loyer attendu[/]({_currentSimulation.Property.MensualRent})?");

        _currentSimulation.Property.RenovationWork =
            AnsiConsole.Ask<int>(
                $"Quel est le [green] prix estimé des travaux[/]?({_currentSimulation.Property.RenovationWork})");

        _currentSimulation.Property.SetPropertyTax(
            AnsiConsole.Ask<int>(
                $"Quel est le [green] montant de la taxe foncière[/]?({_currentSimulation.Property.PropertyTax})"));

        _currentSimulation.Property.SetCondominiumFeesByMonth(AnsiConsole.Ask<int>(
            $"Quel est le [green] montant des charges estimées par mois[/]? ({_currentSimulation.Property.CondominiumFees / 12})"));
    }

    public void Execute()
    {
        var index = _simulationRepository.AllEstateSimulations
            .FindIndex(s => s.Id == _currentSimulation.Id);
        // TODO - encapsulate update and guarantee cohesive state between Loan and Price 
        _currentSimulation.UpdateLoan();
        _currentSimulation.Evaluate();
        
        _simulationRepository.AllEstateSimulations[index] = _currentSimulation;
        _simulationRepository.Save();
    }
}