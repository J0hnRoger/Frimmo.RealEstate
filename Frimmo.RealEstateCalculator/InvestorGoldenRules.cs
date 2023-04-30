namespace Frimmo.RealEstateCalculator;

public enum InvestorPilar
{
  POSITIV_CASHFLOW,
  VALUE,
  SECURITY,
  LONG_TERM
}

/// <summary>
/// Reminder on the golden rules for a good investor
/// </summary>
public class InvestorGoldenRules
{
    public string Principle;
   // 4 pilliers
   public static string PositivCashFlow = "";

   public InvestorGoldenRules(string principle)
   {
       Principle = principle;
   }
}