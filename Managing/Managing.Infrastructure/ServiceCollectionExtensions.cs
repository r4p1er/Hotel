using Managing.Domain.Interfaces;
using Managing.Infrastructure.Database;
using Managing.Infrastructure.HostedServices;
using Managing.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Managing.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection collection, string connection)
    {
        collection.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
        collection.AddScoped<IRoomsRepository, RoomsRepository>();
        collection.AddHostedService<RabbitHostedService>();

        return collection;
    }
}