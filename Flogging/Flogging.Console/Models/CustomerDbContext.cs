using System.Data.Entity;

namespace Flogging.Console.Models
{
    public class CustomerDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
    }
}
