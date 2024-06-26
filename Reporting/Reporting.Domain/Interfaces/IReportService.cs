using Reporting.Domain.DataObjects;
using Reporting.Domain.Entities;

namespace Reporting.Domain.Interfaces;

public interface IReportService
{
    Task<IEnumerable<ReportDTO>> GetAll();
    Task<ReportDTO> GetById(Guid id);
    Task<ReportDTO> CreateReport(ReportData data);
    Task DeleteReport(Guid id);
}