using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class EmployeeTerritories : EntityBase // My Handlebars Helper: EmployeeTerritories
    {
        public int EmployeeId { get; set; } = default!; // Primary Key
        public string TerritoryId { get; set; } = default!; // Primary Key

        public virtual Employee Employee { get; set; } = default!;
        public virtual Territory Territory { get; set; } = default!;
    }
}
