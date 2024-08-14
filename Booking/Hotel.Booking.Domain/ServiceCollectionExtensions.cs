using FluentValidation;
using Hotel.Booking.Domain.Abstractions;
using Hotel.Booking.Domain.Models;
using Hotel.Booking.Domain.Services;
using Hotel.Booking.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Booking.Domain;

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