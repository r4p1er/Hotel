using Microsoft.EntityFrameworkCore;
using Reporting.Domain.Entities;

namespace Reporting.Infrastructure.Database;

public class ApplicationContext : DbContext
{
    public DbSet<Report> Reports { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Report>()
            .Property(x => x.Data)
            .HasColumnType("jsonb");
    }
}