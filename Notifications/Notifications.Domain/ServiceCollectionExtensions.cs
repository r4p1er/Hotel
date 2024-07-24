using Microsoft.Extensions.DependencyInjection;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;
using Notifications.Domain.Services;

namespace Notifications.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddNotifications(this IServiceCollection collection,
        NotificationServiceOptions options)
    {
        collection.AddSingleton<NotificationServiceOptions>(provider => options);
        collection.AddScoped<INotificationService, NotificationService>();

        return collection;
    }
}