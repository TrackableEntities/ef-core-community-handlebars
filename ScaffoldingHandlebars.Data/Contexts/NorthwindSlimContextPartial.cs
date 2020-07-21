using Microsoft.EntityFrameworkCore;
using ScaffoldingHandlebars.Entities.Models;
using System;

namespace ScaffoldingHandlebars.Entities
{
    public partial class NorthwindSlimContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .Property(e => e.Country)
                .HasConversion(
                    v => v.ToString(),
                    v => (Country)Enum.Parse(typeof(Country), v));
        }
    }
}
