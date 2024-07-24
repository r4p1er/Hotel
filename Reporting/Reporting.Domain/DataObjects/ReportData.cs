namespace Reporting.Domain.DataObjects;

/// <summary>
/// Данные для создания нового отчета
/// </summary>
public class ReportData
{
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
}