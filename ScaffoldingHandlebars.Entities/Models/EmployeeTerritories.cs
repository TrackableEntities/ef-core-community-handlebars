using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class EmployeeTerritories : EntityBase
    {
        public int EmployeeId { get; set; } = default!;
        public string TerritoryId { get; set; } = default!;

        public virtual Employee Employee { get; set; } = default!;
        public virtual Territory Territory { get; set; } = default!;
    }
}
