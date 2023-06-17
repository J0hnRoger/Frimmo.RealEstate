namespace Frimmo.RealEstateCalculator;

public class EvaluationResult
{
    public List<EvaluationRule> Rules { get; set; } = new();
}

public class EvaluationRule
{
    public string Description { get; set; }
    public bool IsValid { get; set; }
    public string Explanation { get; set; }
    public EvaluationRule(string description, bool isValid, string explanation)
    {
        Description = description;
        IsValid = isValid;
        Explanation = explanation;
    }
}