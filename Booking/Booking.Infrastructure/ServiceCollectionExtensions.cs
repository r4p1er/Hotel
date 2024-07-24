using Booking.Domain.Interfaces;
using Booking.Infrastructure.Database;
using Booking.Infrastructure.RabbitConsumers;
using Booking.Infrastructure.Services;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection collection, string connection)
    {
        collection.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
        collection.AddScoped<ITicketsRepository, TicketsRepository>();

        return collection;
    }

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