using Hotel.Shared.Extensions;
using Identity.Infrastructure;
using Identity.Infrastructure.Database;

namespace Identity.Api;

/// <summary>
/// Стартовый класс приложения
/// </summary>
public class Program
{
    /// <summary>
    /// Стартовый метод приложения
    /// </summary>
    /// <param name="args">Параметры командной строки</param>
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