using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using BudgetPilot.Web.Models;
using System.Security.Claims;

namespace BudgetPilot.Web.Components.Account;

public class IdentityRevalidatingAuthenticationStateProvider : ServerAuthenticationStateProvider
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<IdentityRevalidatingAuthenticationStateProvider> _logger;

    public IdentityRevalidatingAuthenticationStateProvider(
        IServiceScopeFactory scopeFactory,
        ILoggerFactory loggerFactory)
    {
        _scopeFactory = scopeFactory;
        _logger = loggerFactory.CreateLogger<IdentityRevalidatingAuthenticationStateProvider>();
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Get the authentication state
        var authState = await base.GetAuthenticationStateAsync();

        // Validate the user is still valid
        if (authState.User.Identity?.IsAuthenticated == true)
        {
            using var scope = _scopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var user = await userManager.GetUserAsync(authState.User);
            if (user == null)
            {
                // User no longer exists
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        return authState;
    }
}