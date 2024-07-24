using Booking.Domain;
using Booking.Infrastructure;
using Hotel.Shared.Middlewares;

namespace Booking.Api;

/// <summary>
/// Конфигурация работы приложения
/// </summary>
/// <param name="configuration">Конфигурационные данные приложения</param>
public class Startup(IConfiguration configuration)
{
    /// <summary>
    /// Добавление всех необходимых сервисов в DI контейнер
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
    /// Сконфигурировать работу приложения
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