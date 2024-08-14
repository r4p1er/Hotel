using FluentValidation;
using Hotel.Managing.Domain.DataObjects;
using Hotel.Managing.Domain.Interfaces;
using Hotel.Managing.Domain.Services;
using Hotel.Managing.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Managing.Domain;

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
        collection.AddScoped<IRoomService, RoomService>();
        collection.AddScoped<IValidator<RoomData>, RoomDataValidator>();

        return collection;
    }
}