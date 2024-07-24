using FluentValidation;
using Identity.Domain.DataObjects;
using Identity.Domain.Interfaces;
using Identity.Domain.Services;
using Identity.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Domain;

/// <summary>
/// Расширения коллекции сервисов DI контейнера
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить сервисы доменного уровня
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddDomain(this IServiceCollection collection)
    {
        collection.AddScoped<IValidator<RegisterData>, RegisterDataValidator>();
        
        return collection;
    }

    /// <summary>
    /// Добавить сервис для работы с пользователями
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    /// <param name="options">Опции сервиса UserService</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddUserService(this IServiceCollection collection, UserServiceOptions options)
    {
        collection.AddSingleton<UserServiceOptions>(provider => options);
        collection.AddScoped<IUserService, UserService>();

        return collection;
    }
}