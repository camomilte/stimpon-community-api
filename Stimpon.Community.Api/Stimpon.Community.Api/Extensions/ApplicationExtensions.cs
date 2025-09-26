// File namespace
namespace Stimpon.Community.Api.Extensions;

// Required namespaces
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;

/// <summary>
/// Contains static application extension methods
/// </summary>
public static class ApplicationExtensions
{
    /// <summary>
    /// Setup services for the application
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static WebApplicationBuilder SetupServices(this WebApplicationBuilder builder)
    {
        // Add controllers
        builder.Services.AddControllers();

        // Add open api support
        builder.Services.AddOpenApi();

        // Setup the authentication scheme
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new()
            {
                #region Issuer
                // We want to validate the issuer from appsettings
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["AuthTokenData:Issuer"],
                #endregion
                #region Audience
                // We want to validate the audience from appsettings
                ValidateAudience = true,
                ValidAudience = builder.Configuration["AuthTokenData:Audience"],
                #endregion
                #region Lifetime
                // We want to validate the lifetime of the token
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
                ValidateLifetime = true,               
                #endregion
                #region Signing key
                // We want to validate the token signature
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthTokenData:Secret"]!))
                #endregion
            };
        });

        // Setup authorization
        builder.Services.AddAuthorization(options =>
        {
            // Create the policies for the minimum role
            foreach (Roles role in Enum.GetValues(typeof(Roles)))
            {
                // Add every possible minimum role policy
                options.AddPolicy($"MINIMUM_ROLE_{role.Text()}", policy =>
                    policy.RequireAssertion(context =>
                    {
                        
                        // Get the role from the token claims
                        if (Enum.TryParse<Roles>(context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value, out Roles userRole))
                            // Check if the role is at least the minimum role required
                            return userRole >= role;

                        // No token was provided probably
                        return false;
                    }));
            }
        });

        // Setup community database context
        builder.Services.AddDbContext<CommunityDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        #region CORS

        // Check if CORS is enabled
        if (bool.Parse(builder.Configuration.GetConnectionString("UseCORS") ?? "false"))
        {
            // Setup the stupid CORS crap
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Allow", policy =>
                {
                    policy.WithOrigins(
                        builder.Configuration.GetSection("CORSOrigins").Get<string[]>()!
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });
            });
        }

        #endregion

        #region Developer
        if (builder.Environment.IsDevelopment())
        {
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.ListenAnyIP(int.Parse(builder.Configuration["Debug:ListeningPortHttp"]!)); // lyssnar på alla nätverksadresser
                options.ListenAnyIP(int.Parse(builder.Configuration["Debug:ListeningPortHttps"]!), listenOptions => listenOptions.UseHttps()); // HTTPS
            });
        }
        #endregion

        // Return the builder
        return builder;
    }

    /// <summary>
    /// Setup the application
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static WebApplication SetupApplication(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
        if (app.Environment.IsProduction())
        {
            // Use https redirection
            app.UseHttpsRedirection();
        }

        if (bool.Parse(app.Configuration.GetConnectionString("UseCORS") ?? "false"))
            // Enable CORS
            app.UseCors("Allow");
        
        // Use authorization
        app.UseAuthorization();
        // Map controllers
        app.MapControllers();

        // Return the app
        return app;
    }
}
