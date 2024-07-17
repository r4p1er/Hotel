using System.Text.Json;
using FluentValidation;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Reporting.Domain.DataObjects;
using Reporting.Domain.Entities;
using Reporting.Domain.Interfaces;

namespace Reporting.Domain.Services;

public class ReportService(IReportsRepository repository, IValidator<ReportData> validator) : IReportService
{
    public async Task<IEnumerable<ReportDTO>> GetAll()
    {
        return await repository.FindAll().Select(x => new ReportDTO(x)).ToListAsync();
    }

    public async Task<ReportDTO> GetById(Guid id)
    {
        var report = await repository.FindByIdAsync(id);

        if (report == null) throw new NotFoundException("Report not found");

        return new ReportDTO(report);
    }

    public async Task<ReportDTO> CreateReport(ReportData data)
    {
        await validator.ValidateAndThrowAsync(data);

        var report = new Report()
        {
            Id = Guid.NewGuid(),
            Summary = data.Summary,
            From = data.From,
            To = data.To,
            Data = JsonSerializer.Serialize(data.Data.RootElement)
        };

        await repository.AddReportAsync(report);

        return new ReportDTO(report);
    }

    public async Task DeleteReport(Guid id)
    {
        var report = await repository.FindByIdAsync(id);

        if (report == null) throw new NotFoundException("Report not found");

        await repository.RemoveReportAsync(report);
    }
}