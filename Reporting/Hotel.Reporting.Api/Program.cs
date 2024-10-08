using Hotel.Shared.Extensions;
using Hotel.Reporting.Infrastructure.Database;

namespace Hotel.Reporting.Api;

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