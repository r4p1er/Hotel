using Hotel.Shared.Interfaces;
using Hotel.Shared.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShared(this IServiceCollection collection)
    {
        collection.AddScoped<IRabbitService, RabbitService>();

        return collection;
    }
}