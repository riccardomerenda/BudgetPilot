using System;

namespace BudgetPilot.Web.Models;

public abstract class FamilyScopedEntity : AuditableEntity
{
    public Guid FamilyId { get; set; }
    public Family? Family { get; set; }
}


