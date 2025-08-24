using System;
using System.Linq;
using System.Threading.Tasks;
using BudgetPilot.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BudgetPilot.Web.Data.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext db)
    {
        // Seed Family
        if (!await db.Families.AnyAsync(f => f.Id == SeedIds.DemoFamilyId))
        {
            db.Families.Add(new Family
            {
                Id = SeedIds.DemoFamilyId,
                Name = "Demo Family",
                Currency = "EUR",
                Culture = "it-IT",
            });
        }

        // Seed Categories with color and icon
        if (!await db.Categories.IgnoreQueryFilters().AnyAsync(c => c.FamilyId == SeedIds.DemoFamilyId))
        {
            db.Categories.AddRange(
                new Category
                {
                    Id = SeedIds.CategoryEssenzialiId,
                    FamilyId = SeedIds.DemoFamilyId,
                    Name = "Essenziali (50%)",
                    Color = "#1f77b4",
                    Icon = "home"
                },
                new Category
                {
                    Id = SeedIds.CategoryDesideriId,
                    FamilyId = SeedIds.DemoFamilyId,
                    Name = "Desideri (30%)",
                    Color = "#ff7f0e",
                    Icon = "gift"
                },
                new Category
                {
                    Id = SeedIds.CategoryRisparmiId,
                    FamilyId = SeedIds.DemoFamilyId,
                    Name = "Risparmi (20%)",
                    Color = "#2ca02c",
                    Icon = "piggy-bank"
                }
            );
        }

        // Seed Accounts
        if (!await db.Accounts.IgnoreQueryFilters().AnyAsync(a => a.FamilyId == SeedIds.DemoFamilyId))
        {
            db.Accounts.AddRange(
                new Account
                {
                    Id = SeedIds.AccountContoPrincipaleId,
                    FamilyId = SeedIds.DemoFamilyId,
                    Name = "Conto Principale"
                },
                new Account
                {
                    Id = SeedIds.AccountCartaId,
                    FamilyId = SeedIds.DemoFamilyId,
                    Name = "Carta"
                },
                new Account
                {
                    Id = SeedIds.AccountContantiId,
                    FamilyId = SeedIds.DemoFamilyId,
                    Name = "Contanti"
                }
            );
        }

        await db.SaveChangesAsync();
    }
}


