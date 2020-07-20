using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class CustomerSetting : EntityBase
    {
        public string CustomerId { get; set; }
        public string Setting { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
