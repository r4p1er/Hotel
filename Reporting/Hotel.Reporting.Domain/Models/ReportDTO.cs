using System.Text.Json;
using Hotel.Reporting.Domain.Entities;

namespace Hotel.Reporting.Domain.Models;

/// <summary>
/// DTO для отпаравки отчета клиенту
/// </summary>
public class ReportDTO
{
    /// <summary>
    /// Идентификатор отчета
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Название отчета
    /// </summary>
    public string Summary { get; set; }
    
    /// <summary>
    /// Начало периода отчета
    /// </summary>
    public DateTime From { get; set; }
    
    /// <summary>
    /// Конец периода отчета
    /// </summary>
    public DateTime To { get; set; }
    
    /// <summary>
    /// Данные отчета
    /// </summary>
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