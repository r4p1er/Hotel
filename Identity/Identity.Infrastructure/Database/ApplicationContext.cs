using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Database;

/// <summary>
/// Контекст БД
/// </summary>
/// <param name="options">Опции контекста БД</param>
public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    /// <summary>
    /// Коллекция пользователей
    /// </summary>
    public DbSet<User> Users { get; set; }
}