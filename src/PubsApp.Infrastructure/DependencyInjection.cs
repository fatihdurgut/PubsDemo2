using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PubsApp.Core.Interfaces;
using PubsApp.Infrastructure.Data;
using PubsApp.Infrastructure.Repositories;

namespace PubsApp.Infrastructure;

/// <summary>
/// Extension methods for configuring Infrastructure layer services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Infrastructure layer services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The application configuration</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Skip DbContext registration if already registered (e.g., for testing)
        var existingDbContextOptions = services.FirstOrDefault(d => d.ServiceType == typeof(DbContextOptions<PubsDbContext>));
        if (existingDbContextOptions == null)
        {
            // Get connection string from configuration or environment variable
            var connectionString = configuration.GetConnectionString("PubsDatabase") 
                ?? Environment.GetEnvironmentVariable("PUBS_CONNECTION_STRING")
                ?? throw new InvalidOperationException(
                    "Connection string 'PubsDatabase' not found in configuration or PUBS_CONNECTION_STRING environment variable.");

            // Register DbContext
            services.AddDbContext<PubsDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);
                    sqlOptions.CommandTimeout(30);
                });
            });
        }

        // Register repositories
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<ITitleRepository, TitleRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();

        // Register Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
