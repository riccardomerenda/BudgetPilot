using System;

namespace BudgetPilot.Web.Models;

public class Rule : FamilyScopedEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Contains { get; set; }
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 0;
}


