using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class EmployeeTerritories : EntityBase
    {
        public int EmployeeId { get; set; }
        public string TerritoryId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Territory Territory { get; set; }
    }
}
