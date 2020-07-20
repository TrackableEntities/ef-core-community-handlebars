using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class OrderDetail : EntityBase
    {
        public int OrderDetailId { get; set; } = default!;
        public int OrderId { get; set; } = default!;
        public int ProductId { get; set; } = default!;
        public decimal UnitPrice { get; set; } = default!;
        public short Quantity { get; set; } = default!;
        public float Discount { get; set; } = default!;

        public virtual Order Order { get; set; } = default!;
        public virtual Product Product { get; set; } = default!;
    }
}
