using Frimmo.RealEstateCalculator;
using Newtonsoft.Json;
using Spectre.Console;

namespace Frimmo.Console.Infrastructure;

public class EstatePropertyRepository
{
    private readonly string _dbPath;
    private List<EstateProperty> _allEstateProperties = new List<EstateProperty>();

    public EstatePropertyRepository(string dbPath)
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

    public EstateProperty Get(int id)
    {
        return _allEstateProperties.First(ep => ep.Id == id);
    }
    
    public void Save()
    {
        File.WriteAllText(_dbPath, JsonConvert.SerializeObject(_allEstateProperties)); 
    }

    public void Load()
    {
        string content = File.ReadAllText(_dbPath); 
        if (content == null) 
            throw new ArgumentNullException($"Aucune base JSON trouvée: {_dbPath}");
        _allEstateProperties = JsonConvert.DeserializeObject<List<EstateProperty>>(content) ?? new List<EstateProperty>();
    }
}