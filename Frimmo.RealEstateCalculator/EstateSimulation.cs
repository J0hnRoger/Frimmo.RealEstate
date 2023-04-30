using System.Data;
using Frimmo.RealEstateCalculator.Tests;

namespace Frimmo.RealEstateCalculator;

public class EstateSimulation
{
    public Loan Loan { get; set; }
    public Market Market { get; }
    public EstateProperty Property { get; set; }
    public double CashFlow { get; set; }
    public double IdealPriceForPositiveCashflow { get; set; }

    public EstateSimulation(EstateProperty property, Loan loan, Market market)
    {
        Property = property;
        Loan = loan;
        Market = market;
    }

    /// <summary>
    /// Retourne une note sur 10
    /// </summary>
    /// <returns></returns>
    public EvaluationResult Evaluate()
    {
        EvaluationResult evaluation = new EvaluationResult();

        CashFlow = Property.MensualRent - Loan.MonthlyTotalPayments - Property.AllMonthlyTaxes;

        evaluation.Rules.Add(CheckGrossProfitability());
        evaluation.Rules.Add(CheckCashFlowRule());
        evaluation.Rules.Add(CheckBankRule());
        evaluation.Rules.Add(CheckMarketRule());

        return evaluation;
    }

    private EvaluationRule CheckGrossProfitability()
    {
        double grossProfitability = Property.GetGrossProfitability();
        if (grossProfitability < .1f)
            IdealPriceForPositiveCashflow = GetIdealPriceForPositiveCashFlow();

        return new EvaluationRule(
            $"Pas assez rentable: {grossProfitability * 100}% en dessous de 10% annuel brute",
            grossProfitability > .1f, grossProfitability);
    }

    /// <summary>
    /// Return 
    /// </summary>
    /// <returns></returns>
    private double GetIdealPriceForPositiveCashFlow()
    {
        double minimalLoanRepay = Property.MensualRent - Property.AllMonthlyTaxes;
        double idealAmount = Loan.GetPriceForMensuality(minimalLoanRepay);
        idealAmount = idealAmount - Property.RenovationWork - Property.NotariesFees;
        return idealAmount;
    }

    private EvaluationRule CheckMarketRule()
    {
        return new EvaluationRule(
            $"Market Price Rule: Le prix au m² avec travaux doit être inferieur au marché: il est de {Property.GetNetSquareMeterPrice()}€ " +
            $"contre {Market.SquareMeterPrice}€",
            (Property.GetNetSquareMeterPrice() < Market.SquareMeterPrice),
            Property.GetNetSquareMeterPrice() - Market.SquareMeterPrice);
    }

    private EvaluationRule CheckCashFlowRule()
    {
        return new EvaluationRule($"CashFlow Rule: Un cashflow doit être positif: il est de {CashFlow}€",
            (CashFlow > 0),
            CashFlow);
    }

    /// <summary>
    /// La banque considère un bien autofinancé si 70% des loyers couvrent le crédit 
    /// </summary>
    public EvaluationRule CheckBankRule()
    {
        bool isValid = Property.MensualRent * 0.70 > Loan.MonthlyTotalPayments;
        double diff = Property.MensualRent * 0.70 - Loan.MonthlyTotalPayments;
        return new EvaluationRule("Règle de la banque: 70% du loyer ne couvre pas le crédit.",
            isValid,
            diff);
    }
}