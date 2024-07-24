using Managing.Domain.Interfaces;
using Managing.Infrastructure.Database;
using Managing.Infrastructure.RabbitConsumers;
using Managing.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Managing.Infrastructure;

/// <summary>
/// Расширения коллекции сервисов
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
        collection.AddScoped<IRoomsRepository, RoomsRepository>();

        return collection;
    }

    /// <summary>
    /// Добавить сервис для коммуникации с брокером сообщений RabbitMQ
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    /// <param name="host">Адрес хоста брокера сообщений</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddRabbitMq(this IServiceCollection collection, string host)
    {
        collection.AddMassTransit(x =>
        {
            x.AddConsumer<SelectRoomNamesConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host);
                cfg.ConfigureEndpoints(context);
            });
        });

        return collection;
    }
}