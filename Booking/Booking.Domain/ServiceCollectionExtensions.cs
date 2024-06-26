using Booking.Domain.DataObjects;
using Booking.Domain.Interfaces;
using Booking.Domain.Services;
using Booking.Domain.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection collection)
    {
        collection.AddScoped<ITicketService, TicketService>();
        collection.AddScoped<IValidator<TicketData>, TicketDataValidator>();

        return collection;
    }
}