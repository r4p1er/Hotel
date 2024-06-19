using Identity.Domain.DataObjects;
using Identity.Infrastructure.DataObjects;

namespace Identity.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServicesOptions(this IServiceCollection collection, IConfiguration configuration)
    {
        collection.AddSingleton<UserServiceOptions>(provider =>
            configuration.GetRequiredSection("Auth").Get<UserServiceOptions>()!);

        collection.AddSingleton<DataSeederOptions>(provider =>
            configuration.GetRequiredSection("Seeding").Get<DataSeederOptions>()!);

        return collection;
    }
}