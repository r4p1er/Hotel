using Booking.Domain.DataObjects;
using Booking.Domain.Interfaces;
using Booking.Domain.Services;
using Booking.Domain.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Domain;

/// <summary>
/// Расширения для коллекции сервисов DI контейнера
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
        collection.AddScoped<ITicketService, TicketService>();
        collection.AddScoped<IValidator<TicketData>, TicketDataValidator>();

        return collection;
    }
}