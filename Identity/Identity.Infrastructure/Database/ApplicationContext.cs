using Identity.Application.Interfaces;
using Identity.Infrastructure.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Database;

public class ApplicationContext : DbContext
{
    public DbSet<Role> Roles { get; set; }
    
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
        modelBuilder.Entity<Role>().HasData(new List<Role>()
        {
            new Role() { Id = 1, Alias = "User", Description = "Пользователь" },
            new Role() { Id = 2, Alias = "Manager", Description = "Персонал" },
            new Role() { Id = 3, Alias = "Admin", Description = "Администратор" },
            new Role() { Id = 4, Alias = "Service", Description = "Сервис" }
        });

        modelBuilder.Entity<User>().HasData(new List<User>()
        {
            new User()
            {
                Id = Guid.NewGuid(), Name = "Александр", Surname = "Цыганок", Patronymic = "Михайлович",
                Email = "tsyganok2015@gmail.com", PhoneNumber = "88005553535",
                PasswordHash = _hasher.HashPassword(_options.AdminPassword + _options.Pepper), RoleId = 3,
                IsBlocked = false
            },
            new User()
            {
                Id = Guid.NewGuid(), Name = "Service", Surname = "Service", Patronymic = "Service", Email = "Service",
                PhoneNumber = "Service",
                PasswordHash = _hasher.HashPassword(_options.ServicePassword + _options.Pepper),
                RoleId = 4, IsBlocked = false
            }
        });
    }
}