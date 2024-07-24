using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Reporting.Domain.DataObjects;
using Reporting.Domain.Interfaces;
using Reporting.Domain.Services;
using Reporting.Domain.Validators;

namespace Reporting.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection collection)
    {
        collection.AddScoped<IReportService, ReportService>();
        collection.AddScoped<IValidator<ReportData>, ReportDataValidator>();

        return collection;
    }
}