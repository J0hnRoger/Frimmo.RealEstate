namespace Frimmo.RealEstateCalculator;

public class EvaluationResult
{
    public List<EvaluationRule> Rules { get; set; } = new();
}

public class EvaluationRule
{
    public string Description { get; set; }
    public bool IsValid { get; set; }
    public double ActualValue { get; set; }
    public EvaluationRule(string description, bool isValid, double actualValue)
    {
        Description = description;
        IsValid = isValid;
        ActualValue = actualValue;
    }
}