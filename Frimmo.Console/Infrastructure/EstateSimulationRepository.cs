using System.Globalization;
using Frimmo.RealEstateCalculator;
using Newtonsoft.Json;
using Spectre.Console;

namespace Frimmo.Console.Infrastructure;

public class EstateSimulationRepository
{
    private readonly string _dbPath;
    public List<EstateSimulation> AllEstateSimulations = new();

    public EstateSimulationRepository(string dbPath)
    {
        _dbPath = dbPath;
        if (!File.Exists(_dbPath))
        {
            var stream = File.Create(_dbPath);
            stream.Close();
            AnsiConsole.Write($"EstateProperty Repository: pas de base existante - fichier créé:{_dbPath}");
        }
        Load();
    }

    public EstateSimulation Get(int id)
    {
        return AllEstateSimulations.First(es => es.Id == id);
    }

    public void Save()
    {
        File.WriteAllText(_dbPath, JsonConvert.SerializeObject(AllEstateSimulations, new JsonSerializerSettings()
        {
            Culture = CultureInfo.InvariantCulture
        }));
    }

    public void Load()
    {
        string content = File.ReadAllText(_dbPath);
        if (content == null)
            throw new ArgumentNullException($"Aucune base JSON trouvée: {_dbPath}");
        
        AllEstateSimulations = JsonConvert.DeserializeObject<List<EstateSimulation>>(content, new JsonSerializerSettings(){
           Culture =  CultureInfo.InvariantCulture
        }) ?? new List<EstateSimulation>();
        
        for(int i = 0; i < AllEstateSimulations.Count; i++)
        {
            AllEstateSimulations[i].Id = i;
            AllEstateSimulations[i].Evaluate();
        };
    }
}