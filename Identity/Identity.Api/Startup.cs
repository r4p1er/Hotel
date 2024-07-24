using Hotel.Shared.Middlewares;
using Identity.Domain;
using Identity.Domain.DataObjects;
using Identity.Infrastructure;
using Identity.Infrastructure.DataObjects;

namespace Identity.Api;

public class Startup(IConfiguration configuration)
{
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
    }

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