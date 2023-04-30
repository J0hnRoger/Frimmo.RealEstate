using Frimmo.RealEstateCalculator;
using Frimmo.RealEstateCalculator.Tests;
using Spectre.Console;

namespace Frimmo.Console.Commands;

public class ChooseMarketCommand 
{
    private readonly List<Market> _allMarkets;
    private Market _currentMarket;

    public ChooseMarketCommand(List<Market> allMarkets)
    {
        _allMarkets = allMarkets;
    }
    
    public void ParseInput(string[] args)
    {
        // INPUT
        string marketName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Selectionner un marché ?")
            .AddChoiceGroup("Marchés disponibles:", _allMarkets.Select(m => m.ToString())));

        _currentMarket = _allMarkets.First(m => m.ToString() == marketName);
    }

    public Market Execute()
    {
        return _currentMarket;
    }

    public void DisplayResult()
    {
    }
}