using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hotel.Shared.Extensions;

/// <summary>
/// Расширения для IHost
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// Применить непримененные миграции БД
    /// </summary>
    /// <param name="host"><inheritdoc cref="IHost"/></param>
    /// <typeparam name="T"><inheritdoc cref="DbContext"/></typeparam>
    /// <returns><inheritdoc cref="IHost"/></returns>
    public static async Task<IHost> MigrateDatabaseAsync<T>(this IHost host) where T: DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<T>();
            await context.Database.MigrateAsync();
        }

        return host;
    }
}