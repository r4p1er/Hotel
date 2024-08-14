using Hotel.Shared.Middlewares;
using Hotel.Identity.Domain;
using Hotel.Identity.Domain.DataObjects;
using Hotel.Identity.Infrastructure;
using Hotel.Identity.Infrastructure.DataObjects;

namespace Hotel.Identity.Api;

/// <summary>
/// Конфигурация поведения приложения
/// </summary>
/// <param name="configuration">Конфигурационные данные</param>
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

        var userServiceOptions = configuration.GetRequiredSection("Auth").Get<UserServiceOptions>();
        collection.AddUserService(userServiceOptions!);

        var dataSeederOptions = configuration.GetRequiredSection("Seeding").Get<DataSeederOptions>();
        collection.AddDataSeeder(dataSeederOptions!);

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