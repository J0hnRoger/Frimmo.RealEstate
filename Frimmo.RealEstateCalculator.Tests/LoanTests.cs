using FluentAssertions;
using Xunit;

namespace Frimmo.RealEstateCalculator.Tests;

public class LoanTests
{
    [Fact]
    public void Loan_GetMonthlyInterestPayment()
    {
        var loan = new Loan(100000, 25, 0.0259, 0.034);
        loan.MonthlyInterestPayments.Should().Be(453);
    }
    
    [Fact]
    public void Loan_GetMonthlyInsurancePayment()
    {
        var loan = new Loan(180000, 20, 0.0259, 0.0038);
        loan.MonthlyInsurancePayments.Should().Be(57);
        loan = new Loan(100000, 25, 0.0259, 0.0034);
        loan.MonthlyInsurancePayments.Should().Be(28);
    }
    
    [Fact]
    public void Loan_GetTotalMonthlyPayment()
    {
        var loan = new Loan(100000, 25,  0.0259, 0.0034);
        loan.MonthlyTotalPayments.Should().Be(481);
        loan = new Loan(300000, 25, 0.012, 0.0034);
        loan.MonthlyTotalPayments.Should().Be(1243);
    }
}