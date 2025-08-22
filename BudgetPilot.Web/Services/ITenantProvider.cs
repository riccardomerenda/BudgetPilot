using System;

namespace BudgetPilot.Web.Services;

public interface ITenantProvider
{
    Guid FamilyId { get; }
    string? UserId { get; }
}


