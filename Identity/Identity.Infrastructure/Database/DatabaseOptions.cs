using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Database;

public class DatabaseOptions
{
    public DbContextOptions<ApplicationContext> Options { get; set; }
    
    public string AdminPassword { get; set; }
    
    public string ServicePassword { get; set; }
    
    public string Pepper { get; set; }
}