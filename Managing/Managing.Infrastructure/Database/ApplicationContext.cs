using Managing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Managing.Infrastructure.Database;

/// <summary>
/// Контекст БД
/// </summary>
/// <param name="options">Опции контекста БД</param>
public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    /// <summary>
    /// Коллекция номеров отеля
    /// </summary>
    public DbSet<Room> Rooms { get; set; }

    /// <summary>
    /// Добавить ограничение на поле Price
    /// </summary>
    /// <param name="modelBuilder">ModelBuilder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().ToTable(t => t.HasCheckConstraint("ValidPrice", "\"Price\" > 0"));
    }
}