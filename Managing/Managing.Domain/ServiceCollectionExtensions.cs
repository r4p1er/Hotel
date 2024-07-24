using FluentValidation;
using Managing.Domain.DataObjects;
using Managing.Domain.Interfaces;
using Managing.Domain.Services;
using Managing.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Managing.Domain;

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