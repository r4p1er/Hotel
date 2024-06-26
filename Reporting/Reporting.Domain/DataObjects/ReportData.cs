namespace Reporting.Domain.DataObjects;

public class ReportData
{
    public string Summary { get; set; }
    
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }
    
    public string Data { get; set; }
}