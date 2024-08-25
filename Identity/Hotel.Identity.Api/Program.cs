using Hotel.Shared.Extensions;
using Hotel.Identity.Infrastructure;
using Hotel.Identity.Infrastructure.Database;

namespace Hotel.Identity.Api;

/// <summary/>
public class Program
{
    /// <summary/>
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await host.MigrateDatabaseAsync<ApplicationContext>();
        await host.SeedDataAsync();
        
        await host.RunAsync();
    }

    /// <summary>
    /// Создать сконфигурирванный IHostBuilder
    /// </summary>
    /// <param name="args">Параметры командной строки</param>
    /// <returns>Сконфигурирванный IHostBuilder</returns>
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}