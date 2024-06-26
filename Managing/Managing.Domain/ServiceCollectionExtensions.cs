using FluentValidation;
using Managing.Domain.DataObjects;
using Managing.Domain.Interfaces;
using Managing.Domain.Services;
using Managing.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Managing.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection collection)
    {
        collection.AddScoped<IRoomService, RoomService>();
        collection.AddScoped<IValidator<RoomData>, RoomDataValidator>();

        return collection;
    }
}