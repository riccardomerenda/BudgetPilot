using BudgetPilot.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BudgetPilot.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // IMPORTANT: Set schema BEFORE calling base to ensure Identity tables use it
        builder.HasDefaultSchema("identity");

        base.OnModelCreating(builder);

        // Add indexes for performance
        // Use NormalizedEmail for case-insensitive uniqueness (Identity standard)
        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.NormalizedEmail)
            .IsUnique();

        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.CreatedAt);
    }
}