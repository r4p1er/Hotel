using FluentValidation;
using Hotel.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Reporting.Domain.DataObjects;
using Reporting.Domain.Entities;
using Reporting.Domain.Interfaces;

namespace Reporting.Domain.Services;

public class ReportService : IReportService
{
    private readonly IReportsRepository _repository;
    private readonly IValidator<ReportData> _validator;

    public ReportService(IReportsRepository repository, IValidator<ReportData> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<IEnumerable<Report>> GetAll()
    {
        return await _repository.FindAll().ToListAsync();
    }

    public async Task<Report> GetById(Guid id)
    {
        var report = await _repository.FindByIdAsync(id);

        if (report == null) throw new NotFoundException("Report not found");

        return report;
    }

    public async Task<Report> CreateReport(ReportData data)
    {
        await _validator.ValidateAndThrowAsync(data);

        var report = new Report()
        {
            Id = Guid.NewGuid(),
            Summary = data.Summary,
            From = data.From,
            To = data.To,
            Data = data.Data
        };

        await _repository.AddReportAsync(report);

        return report;
    }

    public async Task DeleteReport(Guid id)
    {
        var report = await GetById(id);

        await _repository.RemoveReportAsync(report);
    }
}