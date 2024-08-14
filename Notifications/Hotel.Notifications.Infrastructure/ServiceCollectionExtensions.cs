using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Hotel.Notifications.Domain.Interfaces;
using Hotel.Notifications.Infrastructure.Models;
using Hotel.Notifications.Infrastructure.RabbitConsumers;
using Hotel.Notifications.Infrastructure.Services;

namespace Hotel.Notifications.Infrastructure;

/// <summary>
/// Расширения коллекции сервисов
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить сервисы для коммуникации с брокером сообщений RabbitMQ
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    /// <param name="host">Адрес хоста брокера сообщений</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddRabbitMq(this IServiceCollection collection, string host)
    {
        collection.AddMassTransit(x =>
        {
            x.AddConsumer<BookingTicketConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host);
                cfg.ConfigureEndpoints(context);
            });
        });

        return collection;
    }

    /// <summary>
    /// Добавить сервис для отправки электронных писем
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    /// <param name="options">Опции сервиса для отправки электронных писем</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddEmailSending(this IServiceCollection collection,
        EmailSendingServiceOptions options)
    {
        collection.AddSingleton<EmailSendingServiceOptions>(provider => options);
        collection.AddScoped<IEmailSendingService, EmailSendingService>();

        return collection;
    }
}