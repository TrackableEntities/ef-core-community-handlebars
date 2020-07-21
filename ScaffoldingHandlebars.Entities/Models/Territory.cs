using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class Territory : EntityBase // My Handlebars Helper: Territory
    {
        public Territory()
        {
            EmployeeTerritories = new List<EmployeeTerritories>();
        }

        public string TerritoryId { get; set; } = default!; // Primary Key
        public string TerritoryDescription { get; set; } = default!;

        public virtual List<EmployeeTerritories> EmployeeTerritories { get; set; } = default!;
    }
}
