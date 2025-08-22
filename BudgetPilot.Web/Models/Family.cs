using System;
using System.Collections.Generic;

namespace BudgetPilot.Web.Models;

public class Family : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<Category> Categories { get; set; } = new List<Category>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public ICollection<Rule> Rules { get; set; } = new List<Rule>();
}


