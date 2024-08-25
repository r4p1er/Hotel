using Hotel.Identity.Infrastructure.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hotel.Identity.Infrastructure;

/// <summary>
/// Расширения для IHost
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// Заполнить БД начальными данными
    /// </summary>
    /// <param name="host">IHost</param>
    /// <returns>IHost</returns>
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