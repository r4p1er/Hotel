using FluentValidation;
using Identity.Domain.DataObjects;
using Identity.Domain.Interfaces;
using Identity.Domain.Services;
using Identity.Domain.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Domain;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection collection)
    {
        collection.AddScoped<IValidator<RegisterData>, RegisterDataValidator>();
        
        return collection;
    }

    public static IServiceCollection AddUserService(this IServiceCollection collection, UserServiceOptions options)
    {
        collection.AddSingleton<UserServiceOptions>(provider => options);
        collection.AddScoped<IUserService, UserService>();

        return collection;
    }
}