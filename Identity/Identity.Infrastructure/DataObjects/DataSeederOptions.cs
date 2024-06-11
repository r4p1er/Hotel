namespace Identity.Infrastructure.DataObjects;

public class DataSeederOptions
{
    public string AdminPassword { get; set; }
    
    public string ServicePassword { get; set; }
    
    public string Pepper { get; set; }
}