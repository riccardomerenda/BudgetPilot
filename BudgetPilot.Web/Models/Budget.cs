using System;

namespace BudgetPilot.Web.Models;

public class Budget : FamilyScopedEntity
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = default!;
    public decimal Amount { get; set; }
}


