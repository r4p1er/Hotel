using Reporting.Domain.DataObjects;
using Reporting.Domain.Entities;

namespace Reporting.Domain.Interfaces;

public interface IReportService
{
    Task<IEnumerable<Report>> GetAll();
    Task<Report> GetById(Guid id);
    Task<Report> CreateReport(ReportData data);
    Task DeleteReport(Guid id);
}