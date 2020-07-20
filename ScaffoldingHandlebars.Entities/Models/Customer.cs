using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    /// <summary>
    /// hello table Customer
    /// </summary>
    public partial class Customer : EntityBase
    {
        public Customer()
        {
            Order = new List<Order>();
        }

        public string CustomerId { get; set; } = default!;
        /// <summary>
        /// hello CompanyName
        /// </summary>
        public string CompanyName { get; set; } = default!;
        public string? ContactName { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }

        public virtual CustomerSetting CustomerSetting { get; set; } = default!;
        public virtual List<Order> Order { get; set; } = default!;
    }
}
