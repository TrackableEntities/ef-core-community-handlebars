using ScaffoldingHandlebars.Entities;
using System;
using System.Linq;

namespace ScaffoldingHandlebars.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new NorthwindSlimContext())
            {
                var employees = context.Employee.Select(e =>
                    new { Name = $"{e.FirstName} {e.LastName}", e.City, e.Country });
                foreach (var e in employees)
                {
                    Console.WriteLine($"{e.Name} {e.City} {e.Country} ");
                }
            }
        }
    }
}
