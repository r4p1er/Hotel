using Hotel.Booking.Infrastructure.Database;
using Hotel.Shared.Extensions;

namespace Hotel.Booking.Api;

/// <summary/>
public class Program
{
    /// <summary/>
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