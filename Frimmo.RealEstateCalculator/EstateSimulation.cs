using Frimmo.RealEstateCalculator.Tests;

namespace Frimmo.RealEstateCalculator;

public class EstateSimulation
{
    public Loan Loan { get; set; }
    public Market Market { get; }
    public EstateProperty Property { get; set; }
    public double CashFlow { get; set; }
    public double IdealPriceForPositiveCashflow { get; set; }
    public EvaluationResult Result { get; set; }
    public int Id { get; set; }

    public EstateSimulation(EstateProperty property, Loan loan, Market market)
    {
        Property = property;
        Loan = loan;
        Market = market;
    }

    public override string ToString()
    {
        return $"{Property?.Description} - {Property.Price}€ - {Property.GetGrossProfitability()}% de renta brute";
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
        evaluation.Rules.Add(CheckNetProfitability());
        evaluation.Rules.Add(CheckCashFlowRule());
        evaluation.Rules.Add(CheckBankRule());
        evaluation.Rules.Add(CheckMarketRule());

        Result = evaluation;

        return evaluation;
    }

    private EvaluationRule CheckGrossProfitability()
    {
        double grossProfitability = Property.GetGrossProfitability();
        bool isValid = grossProfitability > .1f;

        IdealPriceForPositiveCashflow = GetIdealPriceForPositiveCashFlow();

        string explanation = (isValid)
            ? $"{Math.Round(grossProfitability * 100, 2)}% au dessus des 10% annuel brute"
            : $"{Math.Round(grossProfitability * 100, 2)}% en dessous des 10% annuel brute";

        return new EvaluationRule(
            $"Rentabilité brute ",
            isValid, explanation);
    }

    /// <summary>
    /// Evalue la renta net - après travaux
    /// </summary>
    /// <returns></returns>
    private EvaluationRule CheckNetProfitability()
    {
        double netProfitability = Property.GetNetProfitability();
        bool isValid = netProfitability > .1f;

        IdealPriceForPositiveCashflow = GetIdealPriceForPositiveCashFlow();

        string explanation = (isValid)
            ? $"{Math.Round(netProfitability * 100, 2)}% au dessus des 10% annuel net"
            : $"{Math.Round(netProfitability * 100, 2)}% en dessous des 10% annuel net(travaux/frais de notaire/frais d'agence)";

        return new EvaluationRule(
            $"Rentabilité net ",
            isValid, explanation);
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
        bool isValid = Property.GetNetSquareMeterPrice() < Market.SquareMeterPrice;
        string explanation = (isValid)
            ? $"[green]{Property.GetNetSquareMeterPrice()}€/m²( +{Property.GetNetSquareMeterPrice() - Market.SquareMeterPrice}€/m²)[/]"
            : $"[red]{Property.GetNetSquareMeterPrice()}€/m²({Market.SquareMeterPrice - Property.GetNetSquareMeterPrice()}€/m²)[/]";

        return new EvaluationRule(
            $"Market Price Rule: Le prix au m² avec travaux doit être inferieur au marché: il est de {Property.GetNetSquareMeterPrice()}€ " +
            $"contre {Market.SquareMeterPrice}€",
            isValid, explanation);
    }

    private EvaluationRule CheckCashFlowRule()
    {
        bool isValid = CashFlow > 0;
        string explanation = (isValid)
            ? $"[bold green]{CashFlow}€[/]"
            : $"[bold red]{CashFlow}€[/]";
        return new EvaluationRule($"CashFlow Rule: Un cashflow doit être positif: il est de {CashFlow}€",
            (CashFlow > 0), explanation);
    }

    /// <summary>
    /// La banque considère un bien autofinancé si 70% des loyers couvrent le crédit 
    /// </summary>
    public EvaluationRule CheckBankRule()
    {
        bool isValid = Property.MensualRent * 0.70 > Loan.MonthlyTotalPayments;
        string explanation =
            $"Le remboursement représente {Math.Round(Loan.MonthlyTotalPayments / Property.MensualRent * 100, 2)}% des loyers";

        return new EvaluationRule("Règle de la banque: 70% du loyer ne couvre pas le crédit.",
            isValid,
            explanation);
    }

    public void UpdateLoan()
    {
        Loan = new Loan((int) Property.FullPrice, Loan.DurationInMonths / 12, Loan.InterestRate, Loan.InsuranceRate);
    }
}