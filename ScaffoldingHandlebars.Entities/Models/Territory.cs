using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class Territory : EntityBase
    {
        public Territory()
        {
            EmployeeTerritories = new List<EmployeeTerritories>();
        }

        public string TerritoryId { get; set; } = default!;
        public string TerritoryDescription { get; set; } = default!;

        public virtual List<EmployeeTerritories> EmployeeTerritories { get; set; } = default!;
    }
}
