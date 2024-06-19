using Managing.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Managing.Infrastructure.Database;

public class ApplicationContext : DbContext
{
    public DbSet<Room> Rooms { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Room>().ToTable(t => t.HasCheckConstraint("ValidPrice", "Price > 0"));
    }
}