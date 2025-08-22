using BudgetPilot.Web.Models;
using BudgetPilot.Web.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BudgetPilot.Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    private readonly ITenantProvider _tenant;

    // Runtime constructor: uses actual tenant provider
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantProvider tenant)
        : base(options)
    {
        _tenant = tenant;
    }

    // Design-time / tests constructor: uses a null tenant provider
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        _tenant = new NullTenantProvider();
    }

    public DbSet<Family> Families => Set<Family>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<Rule> Rules => Set<Rule>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // IMPORTANT: Set schema BEFORE calling base to ensure Identity tables use it
        builder.HasDefaultSchema("identity");

        base.OnModelCreating(builder);

        // Core schema mapping
        const string core = "core";
        builder.Entity<Family>().ToTable("families", core);
        builder.Entity<Account>().ToTable("accounts", core);
        builder.Entity<Category>().ToTable("categories", core);
        builder.Entity<Transaction>().ToTable("transactions", core);
        builder.Entity<Budget>().ToTable("budgets", core);
        builder.Entity<Rule>().ToTable("rules", core);

        // Identity indexes
        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.NormalizedEmail)
            .IsUnique();

        builder.Entity<ApplicationUser>()
            .HasIndex(u => u.CreatedAt);

        // Relationships
        builder.Entity<ApplicationUser>()
            .HasOne(u => u.Family)
            .WithMany(f => f.Users)
            .HasForeignKey(u => u.FamilyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Category>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children)
            .HasForeignKey(c => c.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Transaction>()
            .HasOne(t => t.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(t => t.AccountId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Transaction>()
            .HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<Budget>()
            .HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Rule>()
            .HasOne(r => r.Category)
            .WithMany()
            .HasForeignKey(r => r.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes per acceptance criteria
        builder.Entity<Transaction>()
            .HasIndex(t => new { t.FamilyId, t.Date });

        builder.Entity<Budget>()
            .HasIndex(b => new { b.FamilyId, b.Date });

        builder.Entity<Transaction>()
            .HasIndex(t => new { t.FamilyId, t.ImportHash })
            .IsUnique()
            .HasFilter("import_hash IS NOT NULL");

        // Global query filters for tenant isolation (exclude Family to avoid required nav warnings)
        builder.Entity<Account>().HasQueryFilter(e => e.FamilyId == _tenant.FamilyId);
        builder.Entity<Category>().HasQueryFilter(e => e.FamilyId == _tenant.FamilyId);
        builder.Entity<Transaction>().HasQueryFilter(e => e.FamilyId == _tenant.FamilyId);
        builder.Entity<Budget>().HasQueryFilter(e => e.FamilyId == _tenant.FamilyId);
        builder.Entity<Rule>().HasQueryFilter(e => e.FamilyId == _tenant.FamilyId);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyAudit();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAudit();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void ApplyAudit()
    {
        var now = DateTime.UtcNow;
        var userId = _tenant.UserId;

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = userId;
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = userId;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = now;
                entry.Entity.UpdatedBy = userId;
            }
        }
    }

    private sealed class NullTenantProvider : ITenantProvider
    {
        public Guid FamilyId => Guid.Empty;
        public string? UserId => null;
    }
}