using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class CustomerSetting : EntityBase
    {
        public string CustomerId { get; set; } = default!;
        public string Setting { get; set; } = default!;

        public virtual Customer Customer { get; set; } = default!;
    }
}
