using Hotel.Reporting.Domain.DataObjects;
using Hotel.Reporting.Domain.Entities;

namespace Hotel.Reporting.Domain.Interfaces;

/// <summary>
/// Сервис для работы с отчетами
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Получить все отчеты
    /// </summary>
    /// <returns>Коллекция с DTO отчетов</returns>
    Task<IEnumerable<ReportDTO>> GetAll();
    
    /// <summary>
    /// Получить отчет по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор отчета</param>
    /// <returns>DTO отчета</returns>
    Task<ReportDTO> GetById(Guid id);
    
    /// <summary>
    /// Создать новый отчет
    /// </summary>
    /// <param name="data">Данные нового отчета</param>
    /// <returns>DTO созданного отчета</returns>
    Task<ReportDTO> CreateReport(ReportData data);
    
    /// <summary>
    /// Удалить отчет по его идентификатору
    /// </summary>
    /// <param name="id">Идентификатор отчета</param>
    /// <returns>Task, представляющий текущую задачу</returns>
    Task DeleteReport(Guid id);
}