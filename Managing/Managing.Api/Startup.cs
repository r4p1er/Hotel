using Hotel.Shared.Middlewares;
using Managing.Domain;
using Managing.Infrastructure;

namespace Managing.Api;

/// <summary>
/// Сконфигурировать сервисы и поведение приложения
/// </summary>
/// <param name="configuration">Конфигурационные данные приложения</param>
public class Startup(IConfiguration configuration)
{
    /// <summary>
    /// Сконфигурировать сервисы приложения
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    public void ConfigureServices(IServiceCollection collection)
    {
        collection.AddControllers();
        
        collection.AddEndpointsApiExplorer();
        collection.AddSwagger();

        collection.AddAuth(configuration["Auth:Key"]!);
        
        collection.AddDomain();
        collection.AddInfrastructure(configuration["Connection:Default"]!);
        collection.AddRabbitMq(configuration["Rabbit:Host"]!);
    }

    /// <summary>
    /// Сконфигурировать поведение приложения
    /// </summary>
    /// <param name="app">IApplicationBuilder</param>
    /// <param name="environment">IWebHostEnvironment</param>
    public void Configure(IApplicationBuilder app, IWebHostEnvironment environment)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();
        
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}