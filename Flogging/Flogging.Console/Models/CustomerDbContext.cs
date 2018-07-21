using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using Flogging.Data.CustomEntityFramework;

namespace Flogging.Console.Models
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext()
        {
            DbInterception.Add(new FloggerEfInterceptor());
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
