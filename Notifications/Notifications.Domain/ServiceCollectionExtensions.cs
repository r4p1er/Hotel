using Microsoft.Extensions.DependencyInjection;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;
using Notifications.Domain.Services;

namespace Notifications.Domain;

/// <summary>
/// Расширения коллекции сервисов
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить сервис NotificationService
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    /// <param name="options">Опции сервиса NotificationService</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddNotifications(this IServiceCollection collection,
        NotificationServiceOptions options)
    {
        collection.AddSingleton<NotificationServiceOptions>(provider => options);
        collection.AddScoped<INotificationService, NotificationService>();

        return collection;
    }
}