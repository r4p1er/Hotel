using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Reporting.Domain.DataObjects;
using Reporting.Domain.Interfaces;
using Reporting.Domain.Services;
using Reporting.Domain.Validators;

namespace Reporting.Domain;

/// <summary>
/// Расширения коллекции сервисов
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить сервисы доменного слоя
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddDomain(this IServiceCollection collection)
    {
        collection.AddScoped<IReportService, ReportService>();
        collection.AddScoped<IValidator<ReportData>, ReportDataValidator>();

        return collection;
    }
}