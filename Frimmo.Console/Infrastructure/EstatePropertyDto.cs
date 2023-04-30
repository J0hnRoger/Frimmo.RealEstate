namespace Frimmo.Console.Infrastructure;

public class EstatePropertyDto
{
    public int Id { get; set; }
    public string Location { get; set; }
    public string ExactAdress { get; set; }
    public string AdUrl { get; set; }
    public DateTime Created { get; set; } 
    
    public double Price { get; private set; }
    public int Surface { get; }
    public double MensualRent { get; private set; }
    public double AgencyFeesPercent { get; set; }
    public double RenovationWork { get; set; }
    public int PropertyTax { get; internal set; }
    public int CondominiumFees { get; set; }
    
}