using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Notifications.Domain.Interfaces;
using Notifications.Infrastructure.Models;
using Notifications.Infrastructure.RabbitConsumers;
using Notifications.Infrastructure.Services;

namespace Notifications.Infrastructure;

public static class ServiceCollectionExtensions
{
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

    public static IServiceCollection AddEmailSending(this IServiceCollection collection,
        EmailSendingServiceOptions options)
    {
        collection.AddSingleton<EmailSendingServiceOptions>(provider => options);
        collection.AddScoped<IEmailSendingService, EmailSendingService>();

        return collection;
    }
}