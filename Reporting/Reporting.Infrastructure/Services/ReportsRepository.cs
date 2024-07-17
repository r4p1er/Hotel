using Microsoft.EntityFrameworkCore;
using Reporting.Domain.Entities;
using Reporting.Domain.Interfaces;
using Reporting.Infrastructure.Database;

namespace Reporting.Infrastructure.Services;

public class ReportsRepository(ApplicationContext context) : IReportsRepository
{
    public IQueryable<Report> FindAll()
    {
        return context.Reports;
    }

    public async Task<Report?> FindByIdAsync(Guid id)
    {
        return await context.Reports.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddReportAsync(Report report)
    {
        await context.Reports.AddAsync(report);

        await context.SaveChangesAsync();
    }

    public async Task RemoveReportAsync(Report report)
    {
        context.Remove(report);

        await context.SaveChangesAsync();
    }
}