using Frimmo.Console.Infrastructure;
using Frimmo.RealEstateCalculator.Tests;
using Spectre.Console;

namespace Frimmo.Console.Commands;

public class ChooseMarketCommand 
{
    private readonly MarketRepository _marketRepository;
    private Market _currentMarket;

    public ChooseMarketCommand(MarketRepository marketRepository)
    {
        _marketRepository = marketRepository;
    }
    
    public void ParseInput(string[] args)
    {
        // INPUT
        string marketName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Selectionner un marché ?")
            .AddChoiceGroup("Marchés disponibles:", _marketRepository.GetAll()
                .Select(m => m.ToString())));

        _currentMarket = _marketRepository.GetAll()
            .First(m => m.ToString() == marketName);
    }

    public Market Execute()
    {
        _marketRepository.CurrentMarket = _currentMarket;
        _marketRepository.Save();
        DisplayResult();
        return _currentMarket;
    }

    public void DisplayResult()
    {
        AnsiConsole.Markup($"Marché actuel: [bold]{_currentMarket}[/]");
    }
}