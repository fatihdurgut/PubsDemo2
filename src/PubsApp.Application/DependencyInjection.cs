using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace PubsApp.Application;

/// <summary>
/// Extension methods for configuring Application layer services
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds Application layer services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        // Register MediatR (scans for handlers automatically)
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(assembly);
        });

        // Register AutoMapper (scans for profiles automatically)
        services.AddAutoMapper(assembly);

        // Register FluentValidation validators
        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
