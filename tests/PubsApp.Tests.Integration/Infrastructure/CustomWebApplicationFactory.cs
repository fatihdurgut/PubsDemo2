using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using PubsApp.Infrastructure.Data;

namespace PubsApp.Tests.Integration.Infrastructure;

/// <summary>
/// Custom WebApplicationFactory for integration testing with in-memory database
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove SQL Server DbContext registration
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<PubsDbContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            // Remove DbContext itself
            var dbContext = services.SingleOrDefault(
                d => d.ServiceType == typeof(PubsDbContext));

            if (dbContext != null)
            {
                services.Remove(dbContext);
            }

            // Add InMemory database for testing
            services.AddDbContext<PubsDbContext>(options =>
            {
                options.UseInMemoryDatabase($"TestDatabase_{Guid.NewGuid()}");
            });
        });

        builder.UseEnvironment("Testing");
    }
}
