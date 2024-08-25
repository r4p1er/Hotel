using FluentValidation;
using Hotel.Reporting.Domain.Abstractions;
using Hotel.Reporting.Domain.Models;
using Hotel.Reporting.Domain.Services;
using Hotel.Reporting.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Reporting.Domain;

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