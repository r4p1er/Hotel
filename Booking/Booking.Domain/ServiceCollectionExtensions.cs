using Booking.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection collection)
    {
        collection.AddScoped<TicketService>();

        return collection;
    }
}