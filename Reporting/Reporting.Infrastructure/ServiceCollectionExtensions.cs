using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Reporting.Domain.Interfaces;
using Reporting.Infrastructure.Database;
using Reporting.Infrastructure.Services;

namespace Reporting.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection collection, string connection)
    {
        collection.AddDbContext<ApplicationContext>(options => options.UseNpgsql(connection));
        collection.AddScoped<IReportsRepository, ReportsRepository>();

        return collection;
    }
}