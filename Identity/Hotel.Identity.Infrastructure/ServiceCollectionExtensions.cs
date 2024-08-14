using Hotel.Identity.Domain.Interfaces;
using Hotel.Identity.Infrastructure.Database;
using Hotel.Identity.Infrastructure.DataObjects;
using Hotel.Identity.Infrastructure.Interfaces;
using Hotel.Identity.Infrastructure.RabbitConsumers;
using Hotel.Identity.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Identity.Infrastructure;

/// <summary>
/// Расширения коллекции сервисов
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить сервисы инфрструктурного слоя
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    /// <param name="connection">Строка подключения к БД</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection collection, string connection)
    {
        collection.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
        collection.AddScoped<IUsersRepository, UsersRepository>();

        return collection;
    }

    /// <summary>
    /// Добавить сервис IDataSeeder
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    /// <param name="options">Опции сервиса IDataSeeder</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddDataSeeder(this IServiceCollection collection, DataSeederOptions options)
    {
        collection.AddSingleton<DataSeederOptions>(provider => options);
        collection.AddScoped<IDataSeeder, DataSeeder>();

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
            x.AddConsumer<SelectUserDataConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host);
                cfg.ConfigureEndpoints(context);
            });
        });

        return collection;
    }
}