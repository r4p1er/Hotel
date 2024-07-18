using Hotel.Shared.Extensions;
using Hotel.Shared.Middlewares;
using Reporting.Domain;
using Reporting.Infrastructure;

namespace Reporting.Api;

public class Startup(IConfiguration configuration)
{
    public void ConfigureServices(IServiceCollection collection)
    {
        collection.AddControllers();
        
        collection.AddEndpointsApiExplorer();
        collection.AddSwagger();

        collection.AddAuth(configuration["Auth:Key"]!);

        collection.AddServicesOptions(configuration);
        collection.AddDomain();
        collection.AddInfrastructure(configuration["Connection:Default"]!);
        collection.AddShared();
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