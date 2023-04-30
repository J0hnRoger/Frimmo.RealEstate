namespace Frimmo.RealEstateCalculator;

/// <summary>
/// Représente un prêt bancaire
/// </summary>
public class Loan 
{
   public int Amount { get; set; }
   public double InterestRate { get; private set; }
   public int DurationInMonths { get; private set; }
   public double InsuranceRate { get; private set; }
   public double MonthlyTotalPayments { get; private set; }
   public double MonthlyInsurancePayments { get; private set; }
   public double MonthlyInterestPayments { get; private set; }
   public Loan(int amount, int durationInYear, double interestRate, double insuranceRate)
   {
      Amount = amount;
      InterestRate = interestRate;
      DurationInMonths = durationInYear * 12;
      InsuranceRate = insuranceRate;
      MonthlyInsurancePayments = GetMonthlyInsurance();
      MonthlyInterestPayments = GetMonthlyInterestPayment();
      MonthlyTotalPayments =  MonthlyInterestPayments + MonthlyInsurancePayments;
   }

   private double GetMonthlyInsurance()
   {
      double exactAmount = (Amount * InsuranceRate) / 12;
      return Math.Round(exactAmount);
   }
   
   private double GetMonthlyInterestPayment()
   {
      double exactAmount = (Amount * (InterestRate / 12)) / (1 - Math.Pow((1 + InterestRate / 12), -DurationInMonths));
      return Math.Round(exactAmount);
   }

   public double GetPriceForMensuality(double idealMensuality)
   {
      double idealAmount = (idealMensuality * (1 - Math.Pow((1 + InterestRate / 12), -DurationInMonths))) / (InterestRate / 12); 
      return Math.Round(idealAmount);
   }
}