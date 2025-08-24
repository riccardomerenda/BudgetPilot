using System;

namespace BudgetPilot.Web.Data.Seed;

public static class SeedIds
{
    // Family
    public static readonly Guid DemoFamilyId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    // Accounts
    public static readonly Guid AccountContoPrincipaleId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    public static readonly Guid AccountCartaId = Guid.Parse("33333333-3333-3333-3333-333333333333");
    public static readonly Guid AccountContantiId = Guid.Parse("44444444-4444-4444-4444-444444444444");

    // Categories (50/30/20)
    public static readonly Guid CategoryEssenzialiId = Guid.Parse("55555555-5555-5555-5555-555555555555"); // 50
    public static readonly Guid CategoryDesideriId = Guid.Parse("66666666-6666-6666-6666-666666666666");   // 30
    public static readonly Guid CategoryRisparmiId = Guid.Parse("77777777-7777-7777-7777-777777777777");   // 20
}


