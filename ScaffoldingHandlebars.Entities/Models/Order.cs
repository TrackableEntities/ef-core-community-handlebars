using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    public partial class Order : EntityBase
    {
        public Order()
        {
            OrderDetail = new List<OrderDetail>();
        }

        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual List<OrderDetail> OrderDetail { get; set; }
    }
}
