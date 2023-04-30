using FluentAssertions;
using Xunit;

namespace Frimmo.RealEstateCalculator.Tests;

public class EstatePropertyTests
{
    
    // - Si j’achète un studio 150 000€ , qui est loué 650€ / mois dans l’annonce, je dois obtenir une renta brute de 650 * 12 / 150000 * 100 = 5,2%
    // - Si j’achète un immeuble 100 000€ , qui est constitué de 3 T1, chacun loué 350€ / mois -  dans l’annonce, je dois obtenir une renta brute de (350+350+350) * 12 / 100 000 * 100 = 12,6%, beaucoup mieux (on peut déjà imaginer qu’il doit y avoir des travaux, mais c’est un bon point de départ pour dégrossir nos listes d’annonces)
    // - Si j’achète un immeuble 0€, et que je touche 100€ de loyer … je dois gérer la classique division / 0 aussi, tant que j’y suis.
   
    // TODO
    // - Si je précise un prix hors frais d'agence (environ 5-8% en général), hors frais de notaires (environ 7-8%), permettre de les renseigner, voir des les générer à partir de ces moyennes  
    [Fact]
    public void EstateProperty_Contains_GrossYield()
    {
        var estateProperty = new EstateProperty(150000, 100);
        estateProperty.SetMensualRentPrice(650); 
        estateProperty.GrossYieldPercent.Should().Be(5.2);
    }
    
    // - Les frais de notaires sont d'environ 7% en Vendée
    [Fact]
    public void EstateProperty_SetNotariesFees()
    {
        var estateProperty = GetTestEstateProperty();
        estateProperty.NotariesFees.Should().Be(7000);
    }

    [Fact]
    public void EstateProperty_SetRenovationsWork()
    {
        var estateProperty = GetTestEstateProperty();
        estateProperty.SetRenovationWorks(100000);
        estateProperty.Price.Should().Be(200000);
    }
    
    [Fact]
    public void EstateProperty_SetLoan()
    {
        var estateProperty = GetTestEstateProperty();
        estateProperty.SetRenovationWorks(100000);
        estateProperty.Price.Should().Be(200000);
    }
    
    
    [Fact]
    public void EstateProperty_SetPropertyTax()
    {
        var estateProperty = GetTestEstateProperty();
        estateProperty.SetPropertyTax(1500);
        estateProperty.PropertyTax.Should().Be(1500);
    }
    
    [Fact]
    public void EstateProperty_SetAgencyFees()
    {
        var estateProperty = GetTestEstateProperty();
        estateProperty.SetAgencyFeesPercent(.06);
        estateProperty.FullPrice.Should().Be(113000);
    }
    
    private EstateProperty GetTestEstateProperty()
    {
        return new EstateProperty(100000, 100);
    }

}