using Booking.Domain.Interfaces;
using Booking.Infrastructure.Database;
using Booking.Infrastructure.HostedServices;
using Booking.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection collection, string connection)
    {
        collection.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
        collection.AddScoped<ITicketsRepository, TicketsRepository>();
        collection.AddHostedService<RabbitHostedService>();

        return collection;
    }
}