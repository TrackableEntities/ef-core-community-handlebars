﻿using System;
using System.Collections.Generic;

namespace ScaffoldingHandlebars.Entities.Models
{
    /// <summary>
    /// hello table Customer
    /// </summary>
    public partial class Customer
    {
        public Customer()
        {
            Order = new HashSet<Order>();
        }

        public string CustomerId { get; set; }
        /// <summary>
        /// hello CompanyName
        /// </summary>
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public virtual CustomerSetting CustomerSetting { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}