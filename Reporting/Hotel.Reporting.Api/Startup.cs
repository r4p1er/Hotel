using Hotel.Reporting.Api.Extensions;
using Hotel.Shared.Middlewares;
using Hotel.Reporting.Domain;
using Hotel.Reporting.Infrastructure;

namespace Hotel.Reporting.Api;

/// <summary/>
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