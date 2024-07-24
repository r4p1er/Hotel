using Microsoft.EntityFrameworkCore;
using Reporting.Domain.Entities;

namespace Reporting.Infrastructure.Database;

/// <summary>
/// Контекст БД
/// </summary>
/// <param name="options">Опции контекста БД</param>
public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    /// <summary>
    /// Коллекция отчетов
    /// </summary>
    public DbSet<Report> Reports { get; set; }

    /// <summary>
    /// Изменить тип данных поля Data на jsonb
    /// </summary>
    /// <param name="modelBuilder">ModelBuilder</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Report>()
            .Property(x => x.Data)
            .HasColumnType("jsonb");
    }
}