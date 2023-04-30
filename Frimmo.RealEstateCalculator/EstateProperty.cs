namespace Frimmo.RealEstateCalculator;

public class EstateProperty
{
    private readonly double _notariesFeesPercent =.07;
    public string Description { get; set; }
    public int Price { get; private set; }
    public int Surface { get; }
    public double MensualRent { get; internal set; }
    public double AnnualRent => MensualRent * 12;
    public double AgencyFeesPercent { get; set; }
    public double GrossYieldPercent => Math.Round(AnnualRent / Price * 100, 3);
    public double NotariesFees => Math.Round(Price * _notariesFeesPercent);
    public double AgencyFees => Math.Round(Price * AgencyFeesPercent);
    public double RenovationWork { get; set; }
    public int PropertyTax { get; internal set; }
    
    public int CondominiumFees { get; set; }
    public double FullPrice => Price + NotariesFees + AgencyFees + RenovationWork;
    public double AllMonthlyTaxes =>  CondominiumFees / 12 + PropertyTax / 12;

    public EstateProperty(int price, int surface)
    {
        Price = price;
        Surface = surface;
    }

    public override string ToString()
    {
        return $"{Description} - {FullPrice}€ - {Surface}m² - ";
    }

    public void SetMensualRentPrice(int mensualRentPrice)
    {
        MensualRent = mensualRentPrice;
    }

    public void SetRenovationWorks(double renovationsWorksAmount)
    {
        RenovationWork = renovationsWorksAmount;
    }

    /// <summary>
    /// In Percent
    /// </summary>
    /// <param name="feesPercent"></param>
    public void SetAgencyFeesPercent(double feesPercent)
    {
        AgencyFeesPercent = feesPercent;
    }

    public void SetPropertyTax(int amount)
    {
        PropertyTax = amount;
    }
    
    public void SetCondominiumFeesByMonth(int amount)
    {
        CondominiumFees = amount * 12;
    }

    /// <summary>
    /// Retourne la rentabilité brute du bien
    /// </summary>
    /// <returns></returns>
    public double GetGrossProfitability()
    {
        return 12 * MensualRent / Price;
    }
    
    /// <summary>
    /// Retourne le prix au m² après travaux (net)
    /// </summary>
    public double GetNetSquareMeterPrice()
    {
        return (Price + RenovationWork) / Surface;
    }
}