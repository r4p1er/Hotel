using Identity.Domain.Interfaces;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection collection)
    {
        collection.AddSingleton<IPasswordHasher, PasswordHasher>();
        collection.AddScoped<ApplicationContext>();
        collection.AddScoped<IUsersRepository, UsersRepository>();

        return collection;
    }
}