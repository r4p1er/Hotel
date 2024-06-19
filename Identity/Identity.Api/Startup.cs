using System.Text;
using Identity.Api.Middlewares;
using Identity.Domain;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Identity.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection collection)
    {
        collection.AddControllers();
        collection.AddEndpointsApiExplorer();

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

        collection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth:Key"]!)),
                    ValidateIssuerSigningKey = true
                };
            });
        
        collection.AddAuthorization();

        collection.AddServicesOptions(_configuration);
        collection.AddDomain();
        collection.AddInfrastructure(_configuration["Connection:Default"]!);
    }

    public void Configure(WebApplication app)
    {
        app.UseMiddleware<ErrorHandlingMiddleware>();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();
    }
}