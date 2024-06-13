using Identity.Domain.DataObjects;
using Identity.Infrastructure.DataObjects;

namespace Identity.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServicesOptions(this IServiceCollection collection)
    {
        collection.AddSingleton<UserServiceOptions>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();

            return configuration.GetRequiredSection("Auth").Get<UserServiceOptions>()!;
        });

        collection.AddSingleton<DataSeederOptions>(provider =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            
            return configuration.GetRequiredSection("Seeding").Get<DataSeederOptions>()!;
        });

        return collection;
    }
}