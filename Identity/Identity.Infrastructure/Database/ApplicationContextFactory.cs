using Identity.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Identity.Infrastructure.Database;

public class ApplicationContextFactory : IDesignTimeDbContextFactory<ApplicationContext>
{
    public ApplicationContext CreateDbContext(string[] args)
    {
        var builder = new ConfigurationBuilder();
        builder.AddCommandLine(args);
        builder.AddEnvironmentVariables();
        var config = builder.Build();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
        optionsBuilder.UseNpgsql(config["Connection:Default"]);

        return new ApplicationContext(new DatabaseOptions()
        {
            Options = optionsBuilder.Options,
            AdminPassword = config["Auth:AdminPassword"]!,
            ServicePassword = config["Auth:ServicePassword"]!,
            Pepper = config["Auth:Pepper"]!
        }, new PasswordHasher());
    }
}