using Booking.Infrastructure.Database;
using Hotel.Shared.Extensions;

namespace Booking.Api;

/// <summary>
/// Стартовый класс приложения
/// </summary>
public class Program
{
    /// <summary>
    /// Стартовый метод приложения
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    public static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();
        await host.MigrateDatabaseAsync<ApplicationContext>();
        
        await host.RunAsync();
    }

    /// <summary>
    /// Создать HostBuilder, сконфигурированный для работы в web
    /// </summary>
    /// <param name="args">Аргументы командной строки</param>
    /// <returns>Созданный и сконфигурированный IHostBuilder</returns>
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}