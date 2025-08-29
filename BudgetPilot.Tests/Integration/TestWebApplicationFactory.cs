using System.Collections.Generic;
using System.Linq;
using BudgetPilot.Web.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace BudgetPilot.Tests.Integration;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Test");

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            var overrides = new Dictionary<string, string?>
            {
                ["Database:ApplyMigrations"] = "false",
                ["Database:ApplySeed"] = "false"
            };
            configBuilder.AddInMemoryCollection(overrides);
        });

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Remove all health check related services to avoid conflicts
            var healthCheckServiceTypes = new[]
            {
                typeof(IHealthCheck),
                typeof(HealthCheckService)
            };

            foreach (var serviceType in healthCheckServiceTypes)
            {
                var descriptors = services.Where(d => d.ServiceType == serviceType).ToList();
                foreach (var desc in descriptors)
                {
                    services.Remove(desc);
                }
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("BudgetPilotTests"));

            // Add clean health checks for testing (using different name to avoid conflicts)
            services.AddHealthChecks()
                .AddCheck("test-database", () => HealthCheckResult.Healthy("Test database is healthy"));
        });

        return base.CreateHost(builder);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Intentionally left minimal; configuration is applied in CreateHost
    }
}


