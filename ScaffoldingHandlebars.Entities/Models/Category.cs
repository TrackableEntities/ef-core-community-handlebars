using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class Category : EntityBase
    {
        public Category()
        {
            Product = new List<Product>();
        }

        public int CategoryId { get; set; } = default!;
        public string CategoryName { get; set; } = default!;

        public virtual List<Product> Product { get; set; } = default!;
    }
}
