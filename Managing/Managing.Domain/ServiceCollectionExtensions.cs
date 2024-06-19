using Managing.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Managing.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection collection)
    {
        collection.AddScoped<RoomService>();

        return collection;
    }
}