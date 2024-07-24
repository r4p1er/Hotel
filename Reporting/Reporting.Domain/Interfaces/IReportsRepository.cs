using Reporting.Domain.Entities;

namespace Reporting.Domain.Interfaces;

public interface IReportsRepository
{
    IQueryable<Report> FindAll();
    Task<Report?> FindByIdAsync(Guid id);
    Task AddReportAsync(Report report);
    Task RemoveReportAsync(Report report);
}