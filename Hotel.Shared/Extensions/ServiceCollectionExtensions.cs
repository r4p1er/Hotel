using Hotel.Shared.Interfaces;
using Hotel.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShared(this IServiceCollection collection)
    {
        collection.AddSingleton<IRabbitConnectionService, RabbitConnectionService>();
        collection.AddTransient<IRabbitService, RabbitService>();

        return collection;
    }
}