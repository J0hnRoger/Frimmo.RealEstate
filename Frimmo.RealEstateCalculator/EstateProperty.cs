namespace Frimmo.RealEstateCalculator;

public class EstateProperty
{
    public double FullPrice { get; }

    public double MensualRent { get; internal set; }

    public double GrossYieldPercent => Math.Round(MensualRent * 12 / FullPrice * 100, 3);

    public EstateProperty(int fullPrice)
    {
        FullPrice = fullPrice;
    }

    public void SetMensualRentPrice(int mensualRentPrice)
    {
        MensualRent = mensualRentPrice;
    }
}