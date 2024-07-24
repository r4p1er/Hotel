using System.Text.Json;
using Reporting.Domain.Entities;

namespace Reporting.Domain.DataObjects;

public class ReportDTO
{
    public Guid Id { get; set; }
    
    public string Summary { get; set; }
    
    public DateTime From { get; set; }
    
    public DateTime To { get; set; }
    
    public JsonDocument Data { get; set; }

    public ReportDTO(Report report)
    {
        Id = report.Id;
        Summary = report.Summary;
        From = report.From;
        To = report.To;
        Data = JsonDocument.Parse(report.Data);
    }
}