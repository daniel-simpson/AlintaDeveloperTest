using Alinta.Customers.Models;
using Microsoft.EntityFrameworkCore;

namespace Alinta.Customers.Data
{
    public class CustomerDataContext : DbContext
    {
        public CustomerDataContext(DbContextOptions<CustomerDataContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
