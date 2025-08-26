using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using BudgetPilot.Web.Models;

namespace BudgetPilot.Web.Data.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
    {
        const string email = "demo@budgetpilot.ai";
        const string password = "Demo123!";

        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = "Demo",
                LastName = "User",
                FamilyId = SeedIds.DemoFamilyId,
                IsActive = true
            };

            await userManager.CreateAsync(user, password);
        }
    }
}