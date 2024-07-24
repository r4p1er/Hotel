using Booking.Domain.Interfaces;
using Booking.Infrastructure.Database;
using Booking.Infrastructure.RabbitConsumers;
using Booking.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure;

/// <summary>
/// Расширения для коллекции сервисов DI контейнера
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить сервисы инфраструктурного слоя
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    /// <param name="connection">Строка подключения к БД</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection collection, string connection)
    {
        collection.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
        collection.AddScoped<ITicketsRepository, TicketsRepository>();

        return collection;
    }

    /// <summary>
    /// Добавления сервиса для коммуникации с брокером сообщений RabbitMQ
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    /// <param name="host">Строка с адресом хоста боркера сообщений</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddRabbitMq(this IServiceCollection collection, string host)
    {
        collection.AddMassTransit(x =>
        {
            x.AddConsumer<SelectBookingTicketsConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host);
                cfg.ConfigureEndpoints(context);
            });
        });

        return collection;
    }
}