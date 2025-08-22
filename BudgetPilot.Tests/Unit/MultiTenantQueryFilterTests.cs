using System;
using System.Threading.Tasks;
using BudgetPilot.Web.Data;
using BudgetPilot.Web.Models;
using BudgetPilot.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace BudgetPilot.Tests.Unit;

public class MultiTenantQueryFilterTests
{
    private class FakeTenantProvider : ITenantProvider
    {
        public FakeTenantProvider(Guid familyId, string? userId = "test-user")
        {
            FamilyId = familyId;
            UserId = userId;
        }

        public Guid FamilyId { get; }
        public string? UserId { get; }
    }

    private static ApplicationDbContext NewContext(Guid familyId, string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new ApplicationDbContext(options, new FakeTenantProvider(familyId));
    }

    [Fact]
    public async Task QueryFilters_IsolateByFamily()
    {
        var famA = Guid.NewGuid();
        var famB = Guid.NewGuid();
        var dbName = $"bp-{Guid.NewGuid()}";

        using (var ctx = NewContext(famA, dbName))
        {
            var accA = new Account { Id = Guid.NewGuid(), FamilyId = famA, Name = "A" };
            var accB = new Account { Id = Guid.NewGuid(), FamilyId = famB, Name = "B" };
            ctx.Accounts.AddRange(accA, accB);
            ctx.Transactions.Add(new Transaction { Id = Guid.NewGuid(), FamilyId = famA, AccountId = accA.Id, Date = new DateOnly(2025, 1, 1), Amount = 10 });
            ctx.Transactions.Add(new Transaction { Id = Guid.NewGuid(), FamilyId = famB, AccountId = accB.Id, Date = new DateOnly(2025, 1, 2), Amount = 20 });
            await ctx.SaveChangesAsync();
        }

        using (var ctxA = NewContext(famA, dbName))
        {
            var count = await ctxA.Transactions.CountAsync();
            Assert.Equal(1, count);
            var tx = await ctxA.Transactions.SingleAsync();
            Assert.Equal(famA, tx.FamilyId);
        }

        using (var ctxB = NewContext(famB, dbName))
        {
            var count = await ctxB.Transactions.CountAsync();
            Assert.Equal(1, count);
            var tx = await ctxB.Transactions.SingleAsync();
            Assert.Equal(famB, tx.FamilyId);
        }
    }
}


