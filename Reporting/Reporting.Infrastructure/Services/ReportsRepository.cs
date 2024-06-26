using Microsoft.EntityFrameworkCore;
using Reporting.Domain.Entities;
using Reporting.Domain.Interfaces;
using Reporting.Infrastructure.Database;

namespace Reporting.Infrastructure.Services;

public class ReportsRepository : IReportsRepository
{
    private readonly ApplicationContext _context;

    public ReportsRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public IQueryable<Report> FindAll()
    {
        return _context.Reports;
    }

    public async Task<Report?> FindByIdAsync(Guid id)
    {
        return await _context.Reports.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddReportAsync(Report report)
    {
        await _context.Reports.AddAsync(report);

        await _context.SaveChangesAsync();
    }

    public async Task RemoveReportAsync(Report report)
    {
        _context.Remove(report);

        await _context.SaveChangesAsync();
    }
}