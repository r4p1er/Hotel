using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Domain.Utils;
using Identity.Infrastructure.Database;
using Identity.Infrastructure.DataObjects;
using Identity.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Services;

public class DataSeeder : IDataSeeder
{
    private readonly ApplicationContext _context;
    private readonly DataSeederOptions _options;

    public DataSeeder(ApplicationContext context, DataSeederOptions options)
    {
        _context = context;
        _options = options;
    }
    
    public async Task SeedAsync()
    {
        if (await _context.Users.FirstOrDefaultAsync(x => x.Role == Role.Admin) == null)
        {
            await _context.AddAsync(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Александр",
                Surname = "Цыганок",
                Patronymic = "Михайлович",
                Email = "tsyganok2015@gmail.com",
                PhoneNumber = "88005553535",
                PasswordHash = PasswordHasher.HashPassword(_options.AdminPassword + _options.Pepper),
                Role = Role.Admin,
                IsBlocked = false
            });
        }

        if (await _context.Users.FirstOrDefaultAsync(x => x.Role == Role.Service) == null)
        {
            await _context.AddAsync(new User()
            {
                Id = Guid.NewGuid(),
                Name = "Service",
                Surname = "Service",
                Patronymic = "Service",
                Email = "Service",
                PhoneNumber = "Service",
                PasswordHash = PasswordHasher.HashPassword(_options.ServicePassword + _options.Pepper),
                Role = Role.Service,
                IsBlocked = false
            });
        }

        await _context.SaveChangesAsync();
    }
}