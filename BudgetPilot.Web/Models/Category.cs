using System;
using System.Collections.Generic;

namespace BudgetPilot.Web.Models;

public class Category : FamilyScopedEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? ParentId { get; set; }
    public Category? Parent { get; set; }

    public ICollection<Category> Children { get; set; } = new List<Category>();
}


