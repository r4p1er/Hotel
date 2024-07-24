using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Reporting.Domain.Interfaces;
using Reporting.Infrastructure.Database;
using Reporting.Infrastructure.Services;

namespace Reporting.Infrastructure;

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
        collection.AddScoped<IReportsRepository, ReportsRepository>();

        return collection;
    }

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
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(host);
            });
        });

        return collection;
    }
}