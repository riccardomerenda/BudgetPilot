using System;

namespace BudgetPilot.Web.Models;

public class Transaction : FamilyScopedEntity
{
    public Guid Id { get; set; }
    public DateOnly Date { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public string? ImportHash { get; set; }

    public Guid AccountId { get; set; }
    public Account Account { get; set; } = default!;

    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; }
}


