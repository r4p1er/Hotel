using System.Text;
using Hotel.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Managing.Api;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServicesOptions(this IServiceCollection collection,
        IConfiguration configuration)
    {
        collection.AddSingleton<RabbitOptions>(provider =>
            configuration.GetRequiredSection("Rabbit").Get<RabbitOptions>()!);

        return collection;
    }
    
    public static IServiceCollection AddSwagger(this IServiceCollection collection)
    {
        collection.AddSwaggerGen(x =>
        {
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            
            x.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        return collection;
    }

    public static IServiceCollection AddAuth(this IServiceCollection collection, string key)
    {
        collection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateIssuerSigningKey = true
                };
            });
        
        collection.AddAuthorization();

        return collection;
    }
}