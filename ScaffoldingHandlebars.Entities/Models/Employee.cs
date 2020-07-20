using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class Employee : EntityBase
    {
        public Employee()
        {
            EmployeeTerritories = new List<EmployeeTerritories>();
        }

        public int EmployeeId { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public DateTime? BirthDate { get; set; }
        public DateTime? HireDate { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public virtual List<EmployeeTerritories> EmployeeTerritories { get; set; } = default!;
    }
}
