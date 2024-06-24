using Identity.Domain.Interfaces;
using Identity.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection collection)
    {
        collection.AddScoped<IUserService, UserService>();
        
        return collection;
    }
}