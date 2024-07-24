using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Database;

/// <summary>
/// Контекст БД
/// </summary>
/// <param name="options">Опции контекста БД</param>
public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    /// <summary>
    /// Коллекция заявок на бронирование
    /// </summary>
    public DbSet<Ticket> Tickets { get; set; }

    /// <summary>
    /// Добавление ограничение на столбец Price
    /// </summary>
    /// <param name="modelBuilder">ModelBuilder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>().ToTable(t => t.HasCheckConstraint("ValidPrice", "\"Price\" > 0"));
    }
}