namespace Frimmo.RealEstateCalculator.Tests;

public enum EstatePropertyType
{   
    Neuf,
    Reno
}

/// <summary>
/// Represente le marché dans lequel se situe le bien
/// </summary>
public class Market
{
    public string Location { get; }
    public int SquareMeterPrice { get; private set; }
    public EstatePropertyType PropertyType { get; private set; }

    public Market(string location, int squareMeterPrice, EstatePropertyType propertyType)
    {
        Location = location;
        SquareMeterPrice = squareMeterPrice;
        PropertyType = propertyType;
    }

    public override string ToString()
    {
        return $"{Location} - prix au m²: {SquareMeterPrice}€";
    }
}