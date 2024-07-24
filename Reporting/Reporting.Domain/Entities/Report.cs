namespace Reporting.Domain.Entities;

/// <summary>
/// Отчет
/// </summary>
public class Report
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
    public string Data { get; set; }
}