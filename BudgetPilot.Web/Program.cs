using BudgetPilot.Web.Components;
using BudgetPilot.Web.Components.Account;
using BudgetPilot.Web.Data;
using BudgetPilot.Web.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Serilog;
using Serilog.Events;

// Bootstrap Serilog (console only for early startup)
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting BudgetPilot application on .NET 9");

    var builder = WebApplication.CreateBuilder(args);

    // Configure Serilog (full configuration from appsettings.json)
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
        .Enrich.WithEnvironmentName());

    // Add services
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

    // EF Core with PostgreSQL + Snake Case naming convention
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options
            .UseNpgsql(connectionString, npgsql =>
            {
                npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "public");
            })
            .UseSnakeCaseNamingConvention();

        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        }
    });

    // Check if running under EF Core tools
    if (EF.IsDesignTime)
    {
        // Skip service configuration when running migrations
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options
                .UseNpgsql(connectionString, npgsql =>
                {
                    npgsql.MigrationsHistoryTable("__EFMigrationsHistory", "public");
                })
                .UseSnakeCaseNamingConvention());
    }
    else
    {
        // Identity
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<BudgetPilot.Web.Services.ITenantProvider, BudgetPilot.Web.Services.HttpContextTenantProvider>();
        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;

            // Sign in settings  
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;

            // User settings
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders()
        .AddClaimsPrincipalFactory<BudgetPilot.Web.Services.ApplicationUserClaimsPrincipalFactory>();

        // Add Blazor services
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();

        // Add Authentication State Provider for Blazor
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
        builder.Services.AddScoped<IdentityRevalidatingAuthenticationStateProvider>();

        // Health Checks
        builder.Services.AddHealthChecks()
            .AddNpgSql(connectionString, name: "database");

        // Authorization
        builder.Services.AddAuthorization();
    }

    var app = builder.Build();

    // Skip middleware configuration when running under EF Core tools
    if (!EF.IsDesignTime)
    {
        // Auto-migrate disabled during app startup to avoid impacting tests/runtime
        // Run migrations explicitly via CLI or deployment pipeline

        // Configure pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAntiforgery();

        // Authentication & Authorization
        app.UseAuthentication();
        app.UseAuthorization();

        // Serilog request logging
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            options.GetLevel = (httpContext, elapsed, ex) => ex != null
                ? LogEventLevel.Error
                : elapsed > 500
                    ? LogEventLevel.Warning
                    : LogEventLevel.Debug;
        });

        // Map Blazor components
        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        // Map Identity pages
        app.MapIdentityPages();

        // Health check with explicit response writer
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
                await context.Response.WriteAsync(report.Status.ToString())
        });
        // Optional: run migrations and seed demo data in Development
        // Note: avoid running during design-time or tooling to prevent host abort
        if (!EF.IsDesignTime && !app.Environment.IsEnvironment("Test"))
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<Microsoft.Extensions.Configuration.IConfiguration>();
                var db = services.GetRequiredService<ApplicationDbContext>();

                var applyMigrations = configuration.GetValue<bool>("Database:ApplyMigrations", app.Environment.IsDevelopment());
                var applySeed = configuration.GetValue<bool>("Database:ApplySeed", app.Environment.IsDevelopment());

                if (applyMigrations)
                {
                    db.Database.Migrate();
                }

                if (applySeed)
                {
                    BudgetPilot.Web.Data.Seed.DatabaseSeeder.SeedAsync(db).GetAwaiter().GetResult();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    BudgetPilot.Web.Data.Seed.IdentitySeeder.SeedAsync(userManager).GetAwaiter().GetResult();
                }
            }
        }

        app.Run();
    }
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

// Make Program class accessible for integration tests
public partial class Program { }