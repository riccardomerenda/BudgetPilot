using System;
using System.Collections.Generic;

namespace BudgetPilot.Web.Models;

public class Account : FamilyScopedEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}


