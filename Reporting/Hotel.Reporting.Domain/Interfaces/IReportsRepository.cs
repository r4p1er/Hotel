using Hotel.Reporting.Domain.Entities;

namespace Hotel.Reporting.Domain.Interfaces;

/// <summary>
/// Репозиторий с отчетами
/// </summary>
public interface IReportsRepository
{
    /// <summary>
    /// Получить все отчеты
    /// </summary>
    /// <returns>IQueryable с отчетами</returns>
    IQueryable<Report> FindAll();
    
    /// <summary>
    /// Получить отчет по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор отчета</param>
    /// <returns>Отчет, если не найден - null</returns>
    Task<Report?> FindByIdAsync(Guid id);
    
    /// <summary>
    /// Добавить новый отчет
    /// </summary>
    /// <param name="report">Отчет</param>
    /// <returns>Task, прдеставляющий текущую задачу</returns>
    Task AddReportAsync(Report report);
    
    /// <summary>
    /// Удалить отчет
    /// </summary>
    /// <param name="report">Отчет</param>
    /// <returns>Task, прдеставляющий текущую задачу</returns>
    Task RemoveReportAsync(Report report);
}