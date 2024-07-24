using Identity.Infrastructure.Database;
using Identity.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity.Infrastructure;

public static class HostExtensions
{
    public static async Task<IHost> SeedDataAsync(this IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var dataSeeder = services.GetRequiredService<IDataSeeder>();
            await dataSeeder.SeedAsync();
        }

        return host;
    }
}