using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Identity.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Database;

public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; }

    private readonly IPasswordHasher _hasher;
    private readonly DatabaseOptions _options;

    public ApplicationContext(DatabaseOptions options, IPasswordHasher hasher) : base(options.Options)
    {
        _hasher = hasher;
        _options = options;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(new List<User>()
        {
            new User()
            {
                Id = Guid.NewGuid(), Name = "Александр", Surname = "Цыганок", Patronymic = "Михайлович",
                Email = "tsyganok2015@gmail.com", PhoneNumber = "88005553535",
                PasswordHash = _hasher.HashPassword(_options.AdminPassword + _options.Pepper), Role = Role.Admin,
                IsBlocked = false
            },
            new User()
            {
                Id = Guid.NewGuid(), Name = "Service", Surname = "Service", Patronymic = "Service", Email = "Service",
                PhoneNumber = "Service",
                PasswordHash = _hasher.HashPassword(_options.ServicePassword + _options.Pepper),
                Role = Role.Service, IsBlocked = false
            }
        });
    }
}