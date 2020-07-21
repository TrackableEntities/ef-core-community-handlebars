using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class Category : EntityBase // My Handlebars Helper: Category
    {
        public Category()
        {
            Product = new List<Product>();
        }

        public int CategoryId { get; set; } = default!; // Primary Key
        public string CategoryName { get; set; } = default!;

        public virtual List<Product> Product { get; set; } = default!;
    }
}
