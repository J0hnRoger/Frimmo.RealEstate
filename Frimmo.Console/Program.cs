using System.Reflection;
using AutoMapper;
using Frimmo.Console.Commands;
using Frimmo.Console.Infrastructure;
using Frimmo.RealEstateCalculator;
using Frimmo.RealEstateCalculator.Tests;
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
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false);

IConfiguration config = builder.Build();

// LOADING DATAS
MarketRepository marketRepository = new MarketRepository(_exePath + "/markets.json");
var _currentMarket = marketRepository.CurrentMarket;

AnsiConsole.Markup($"Marché actuel: [red]{_currentMarket}[/]");

if (_currentMarket == null)
{
    var command = new ChooseMarketCommand(marketRepository.GetAll());
    command.ParseInput(args);
    Market selectedMarket = command.Execute();
    marketRepository.CurrentMarket = selectedMarket;
    marketRepository.Save();
    return;
}

var estatePropertyRepository = new EstatePropertyRepository(_exePath + "/db.json");
estatePropertyRepository.Load();

// INPUT COMMANDS
if (args.Length > 0 && args[0] == "--inline")
{
    var command = new CreateNewAnalyzeCommand();
    command.ParseInput(args);
    command.Execute(_currentMarket);
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
    var command = new ChooseMarketCommand(marketRepository.GetAll());
    command.ParseInput(args);
    command.Execute();
    return;
}

if (actionChoice == "Nouvelle analyse")
{
    var command = new CreateNewAnalyzeCommand();
    command.ParseInput(args);
    command.Execute(_currentMarket);
    return;
}

if (actionChoice == "Créer un marché")
{
    return;
}

if (actionChoice == "Compléter une analyse existante")
{
    return;
}