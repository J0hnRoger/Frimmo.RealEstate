using System.Reflection;
using AutoMapper;
using Frimmo.Console.Commands;
using Frimmo.Console.Infrastructure;
using Frimmo.RealEstateCalculator;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

string _exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

AnsiConsole.Write(
    new FigletText("FRIMMO ANALYTICS")
        .LeftJustified()
        .Color(Color.Green));

// SETUP 
var mapperConfiguration = new MapperConfiguration(cfg 
    => cfg.CreateMap<EstateProperty, EstatePropertyDto>());

var builder = new ConfigurationBuilder()
    .SetBasePath(_exePath)
    .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();

// LOADING DATAS
MarketRepository marketRepository = new MarketRepository(_exePath + "/markets.json");
var _currentMarket = marketRepository.CurrentMarket;

AnsiConsole.WriteLine($"Marché actuel: {_currentMarket}");

if (_currentMarket == null)
{
    var command = new ChooseMarketCommand(marketRepository);
    command.ParseInput(args);
    command.Execute();
    return;
}

var _estateSimulationRepository = new EstateSimulationRepository(_exePath + "/db.json");
_estateSimulationRepository.Load();

// INPUT COMMANDS
if (args.Length > 0 && args[0] == "--inline")
{
    var command = new CreateNewSimulationCommand();
    command.ParseInput(args);
    EstateSimulation result = command.Execute(_currentMarket);
    _estateSimulationRepository.AllEstateSimulations.Add(result);
    return;
}
    
var actionChoice = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Que voulez-vous faire ?")
        .AddChoiceGroup("Action", new[]
        {
            "Nouvelle analyse", 
            "Choisir un marché", 
            "Compléter une analyse existante"
        }));

if (actionChoice == "Choisir un marché")
{
    var command = new ChooseMarketCommand(marketRepository);
    command.ParseInput(args);
    command.Execute();
    return;
}

if (actionChoice == "Nouvelle analyse")
{
    var command = new CreateNewSimulationCommand();
    command.ParseInput(args);
    EstateSimulation result = command.Execute(_currentMarket);
    
    var saveCommand = new SaveSimulationCommand(_estateSimulationRepository);
    saveCommand.Execute(result);
    return;
}

if (actionChoice == "Créer un marché")
{
    return;
}

if (actionChoice == "Compléter une analyse existante")
{
    var command = new EditExistingSimulationCommand(_estateSimulationRepository);
    return;
}