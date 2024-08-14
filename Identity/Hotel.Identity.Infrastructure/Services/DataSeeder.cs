using Hotel.Identity.Domain.Entities;
using Hotel.Identity.Domain.Enums;
using Hotel.Identity.Domain.Utils;
using Hotel.Identity.Infrastructure.Abstractions;
using Hotel.Identity.Infrastructure.Database;
using Hotel.Identity.Infrastructure.DataObjects;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Identity.Infrastructure.Services;

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