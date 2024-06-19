using Identity.Infrastructure;

namespace Identity.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var startup = new Startup(builder.Configuration);
        
        startup.ConfigureServices(builder.Services);

        var app = builder.Build();

        startup.Configure(app);

        await app.MigrateDatabaseAsync();
        await app.SeedDataAsync();

        await app.RunAsync();
    }
}