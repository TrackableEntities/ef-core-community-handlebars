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

        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }

        public virtual List<EmployeeTerritories> EmployeeTerritories { get; set; }
    }
}
