using Identity.Domain.Interfaces;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.DataObjects;
using Identity.Infrastructure.Interfaces;
using Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection collection, string connection)
    {
        collection.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
        collection.AddScoped<IUsersRepository, UsersRepository>();

        return collection;
    }

    public static IServiceCollection AddDataSeeder(this IServiceCollection collection, DataSeederOptions options)
    {
        collection.AddSingleton<DataSeederOptions>(provider => options);
        collection.AddScoped<IDataSeeder, DataSeeder>();

        return collection;
    }
}