using System.Reflection;
using AutoMapper;
using Frimmo.Console.Commands;
using Frimmo.Console.Infrastructure;
using Frimmo.RealEstateCalculator;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

// TODO 
// [ ] Ajout du prêt paramétrable
// [ ] Formule prix idéal à revoir
// [ ] Ajouter l'edition d'une analyse 
// [ ] Ajouter la date d'analyse + historisation des prix 

string _exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

AnsiConsole.Write(
    new FigletText("FRIMMO ANALYTICS")
        .LeftJustified()
        .Color(Color.DeepPink2));

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
var _currentLoan = config.GetSection("Loan").Get<LoanConfig>();

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
    EstateSimulation result = command.Execute(_currentMarket, _currentLoan.DurationInYears, _currentLoan.InterestRate,
        _currentLoan.InsuranceRate);

    var saveCommand = new SaveSimulationCommand(_estateSimulationRepository);
    saveCommand.Execute(result);

    return;
}

var actionChoice = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Que voulez-vous faire ?")
        .AddChoiceGroup("Action", new[]
        {
            "Choisir un marché",
            "Nouvelle analyse",
            "Voir toutes les analyses (triées par rentabilité)",
            "Compléter une analyse existante",
            "Supprimer une analyse existante",
        }));

if (actionChoice == "Choisir un marché")
{
    var command = new ChooseMarketCommand(marketRepository);
    command.ParseInput(args);
    command.Execute();
    return;
}

if (actionChoice == "Voir toutes les analyses (triées par rentabilité)")
{
    var command = new GetAllSimulationsCommand(_estateSimulationRepository);
    command.Execute();
    return;
}

if (actionChoice == "Nouvelle analyse")
{
    var command = new CreateNewSimulationCommand();
    command.ParseInput(args);
    EstateSimulation result = command.Execute(_currentMarket, _currentLoan.DurationInYears, _currentLoan.InterestRate,
        _currentLoan.InsuranceRate);

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
    command.ParseInput();
    command.Execute();
    return;
}

if (actionChoice == "Supprimer une analyse existante")
{
    var command = new DeleteExistingSimulationCommand(_estateSimulationRepository);
    command.ParseInput();
    command.Execute();
}