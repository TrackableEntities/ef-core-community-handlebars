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

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? CategoryId { get; set; }
        public decimal? UnitPrice { get; set; }
        public bool Discontinued { get; set; }
        public byte[] RowVersion { get; set; }

        public virtual Category Category { get; set; }
        public virtual List<OrderDetail> OrderDetail { get; set; }
    }
}
