using System.Text.Json;
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

    public async Task<IEnumerable<ReportDTO>> GetAll()
    {
        return await _repository.FindAll().Select(x => new ReportDTO(x)).ToListAsync();
    }

    public async Task<ReportDTO> GetById(Guid id)
    {
        var report = await _repository.FindByIdAsync(id);

        if (report == null) throw new NotFoundException("Report not found");

        return new ReportDTO(report);
    }

    public async Task<ReportDTO> CreateReport(ReportData data)
    {
        await _validator.ValidateAndThrowAsync(data);

        var report = new Report()
        {
            Id = Guid.NewGuid(),
            Summary = data.Summary,
            From = data.From,
            To = data.To,
            Data = JsonSerializer.Serialize(data.Data.RootElement)
        };

        await _repository.AddReportAsync(report);

        return new ReportDTO(report);
    }

    public async Task DeleteReport(Guid id)
    {
        var report = await _repository.FindByIdAsync(id);

        if (report == null) throw new NotFoundException("Report not found");

        await _repository.RemoveReportAsync(report);
    }
}