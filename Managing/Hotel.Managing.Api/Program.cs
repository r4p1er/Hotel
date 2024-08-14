using Hotel.Shared.Extensions;
using Hotel.Managing.Infrastructure.Database;

namespace Hotel.Managing.Api;

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
        
        await host.RunAsync();
    }

    /// <summary>
    /// Создать и сконфигурировать IHostBuilder
    /// </summary>
    /// <param name="args">Параметры командной строки</param>
    /// <returns>Сконфигурированный IHostBuilder</returns>
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}