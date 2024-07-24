namespace Reporting.Domain.Entities;

public class Report
{
    public Guid Id { get; set; }
    
    public string Summary { get; set; }
    
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }
    
    public string Data { get; set; }
}