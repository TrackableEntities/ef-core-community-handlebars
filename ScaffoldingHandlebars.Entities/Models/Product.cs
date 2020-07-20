using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class Product : EntityBase
    {
        public Product()
        {
            OrderDetail = new List<OrderDetail>();
        }

        public int ProductId { get; set; } = default!;
        public string ProductName { get; set; } = default!;
        public int? CategoryId { get; set; }
        public decimal? UnitPrice { get; set; }
        public bool Discontinued { get; set; } = default!;
        public byte[]? RowVersion { get; set; }

        public virtual Category Category { get; set; } = default!;
        public virtual List<OrderDetail> OrderDetail { get; set; } = default!;
    }
}
