namespace Frimmo.RealEstateCalculator;

/// <summary>
/// Représente un prêt bancaire
/// </summary>
public class Loan 
{
   public int Amount { get; set; }
   public double InterestRate { get; set; }
   public int DurationInMonths { get; set; }
   public double InsuranceRate { get; set; }
   public double MonthlyTotalPayments { get; set; }
   public double MonthlyInsurancePayments { get; set; }
   public double MonthlyInterestPayments { get; set; }
   
   public Loan(int amount, int durationInYear, double interestRate, double insuranceRate)
   {
      Amount = amount;
      InterestRate = interestRate;
      DurationInMonths = durationInYear * 12;
      InsuranceRate = insuranceRate;
      MonthlyInsurancePayments = GetMonthlyInsurancePayment();
      MonthlyInterestPayments = GetMonthlyInterestPayment();
      MonthlyTotalPayments =  MonthlyInterestPayments + MonthlyInsurancePayments;
   }

   private double GetMonthlyInsurancePayment()
   {
      double exactAmount = (Amount * InsuranceRate) / 12;
      return Math.Round(exactAmount);
   }
   
   private double GetMonthlyInterestPayment()
   {
      double exactAmount = (Amount * (InterestRate / 12)) / (1 - Math.Pow((1 + InterestRate / 12), -DurationInMonths));
      return Math.Round(exactAmount);
   }

   public double GetFullLoanMensuality()
   {
      return GetMonthlyInsurancePayment() + GetMonthlyInterestPayment();
   }
   
   public double GetPriceForMensuality(double idealMensuality)
   {
      double idealAmount = (idealMensuality * (1 - Math.Pow((1 + InterestRate / 12), -DurationInMonths))) / (InterestRate / 12); 
      return Math.Round(idealAmount);
   }

   public override string ToString()
   {
      return $"Prêt sur {DurationInMonths / 12} ans - {GetFullLoanMensuality()} / mois";
   }
}