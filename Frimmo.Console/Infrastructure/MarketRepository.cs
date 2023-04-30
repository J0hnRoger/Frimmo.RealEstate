using Frimmo.RealEstateCalculator.Tests;
using Newtonsoft.Json;
using Spectre.Console;

namespace Frimmo.Console.Infrastructure;

public class MarketsDto
{
    public Market CurrentMarket { get; set; }
    public List<Market> AllMarkets { get; set; }
}

public class MarketRepository
{
    private readonly string _dbPath;
    private MarketsDto _marketDb;
    public Market CurrentMarket
    {
        get => _marketDb.CurrentMarket;
        set => _marketDb.CurrentMarket = value;
    }

    public MarketRepository(string dbPath)
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

    public Market Get(string location)
    {
        return _marketDb.AllMarkets.First(ep => ep.Location == location);
    }

    public void Save()
    {
        File.WriteAllText(_dbPath, JsonConvert.SerializeObject(_marketDb));
    }

    public void Load()
    {
        string content = File.ReadAllText(_dbPath);
        if (content == null)
            throw new ArgumentNullException($"Aucune base JSON trouvée: {_dbPath}");
        
        _marketDb = JsonConvert.DeserializeObject<MarketsDto>(content) ?? new MarketsDto();
    }

    public List<Market> GetAll()
    {
        return _marketDb.AllMarkets;
    }
}