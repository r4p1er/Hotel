using Booking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Booking.Infrastructure.Database;

public class ApplicationContext : DbContext
{
    public DbSet<Ticket> Tickets { get; set; }
    
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>().ToTable(t => t.HasCheckConstraint("ValidPrice", "\"Price\" > 0"));
    }
}