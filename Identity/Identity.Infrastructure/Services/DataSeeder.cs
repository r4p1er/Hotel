using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Domain.Utils;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.DataObjects;
using Identity.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Services;

/// <inheritdoc cref="IDataSeeder"/>
public class DataSeeder(ApplicationContext context, DataSeederOptions options) : IDataSeeder
{
    /// <inheritdoc cref="IDataSeeder.SeedAsync"/>
    public async Task SeedAsync()
    {
        if (await context.Users.FirstOrDefaultAsync(x => x.Role == Role.Admin) == null)
        {
            await context.AddAsync(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Александр",
                Surname = "Цыганок",
                Patronymic = "Михайлович",
                Email = "tsyganok2015@gmail.com",
                PhoneNumber = "88005553535",
                PasswordHash = PasswordHasher.HashPassword(options.AdminPassword + options.Pepper),
                Role = Role.Admin,
                IsBlocked = false
            });
        }

        if (await context.Users.FirstOrDefaultAsync(x => x.Role == Role.Service) == null)
        {
            await context.AddAsync(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Service",
                Surname = "Service",
                Patronymic = "Service",
                Email = "Service",
                PhoneNumber = "Service",
                PasswordHash = PasswordHasher.HashPassword(options.ServicePassword + options.Pepper),
                Role = Role.Service,
                IsBlocked = false
            });
        }

        await context.SaveChangesAsync();
    }
}