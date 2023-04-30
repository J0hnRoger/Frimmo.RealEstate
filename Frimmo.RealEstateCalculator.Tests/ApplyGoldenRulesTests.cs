using Xunit;

namespace Frimmo.RealEstateCalculator.Tests;

public class ApplyGoldenRulesTests
{
    [Fact]
    public void ApplyGoldenRules()
    {
        var firstRule = new InvestorGoldenRules("Cashflow positif net-net");
        var secondRule = new InvestorGoldenRules("Create value");
        var thirdRule = new InvestorGoldenRules("Liquidité + Revente");
        var fourthRule = new InvestorGoldenRules("Long term");
        // Armes long terms : Je peux déjà jouer, mais il me manquera des cartes - je dois donc me former pour intégrer
        // ces nouvelles cartes
    }
}