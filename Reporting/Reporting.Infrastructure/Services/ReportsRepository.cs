using Microsoft.EntityFrameworkCore;
using Reporting.Domain.Entities;
using Reporting.Domain.Interfaces;
using Reporting.Infrastructure.Database;

namespace Reporting.Infrastructure.Services;

/// <summary>
/// Репозиторий с отчетами. Реализация IReportsRepository
/// </summary>
/// <param name="context">Контекст БД</param>
public class ReportsRepository(ApplicationContext context) : IReportsRepository
{
    /// <inheritdoc cref="IReportsRepository.FindAll"/>
    public IQueryable<Report> FindAll()
    {
        return context.Reports;
    }

    /// <inheritdoc cref="IReportsRepository.FindByIdAsync"/>
    public async Task<Report?> FindByIdAsync(Guid id)
    {
        return await context.Reports.FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc cref="IReportsRepository.AddReportAsync"/>
    public async Task AddReportAsync(Report report)
    {
        await context.Reports.AddAsync(report);

        await context.SaveChangesAsync();
    }

    /// <inheritdoc cref="IReportsRepository.RemoveReportAsync"/>
    public async Task RemoveReportAsync(Report report)
    {
        context.Remove(report);

        await context.SaveChangesAsync();
    }
}