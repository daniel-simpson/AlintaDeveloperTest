using System;
using System.Threading.Tasks;
using Alinta.Customers.Data;
using Alinta.Customers.Models;
using Microsoft.EntityFrameworkCore;

namespace Alinta.Customers.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerDataContext _context;

        public CustomerService(CustomerDataContext context)
        {
            _context = context;
        }

        public async Task AddCustomer(Customer customer)
        {
            await _context.AddAsync(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<Customer> GetCustomer(Guid customerId)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(x => x.Id == customerId);
        }

        public async Task EditCustomer(Customer newCustomer)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(x => x.Id == newCustomer.Id);
            
            if (customer == default(Customer))
            {
                return;
            }

            _context.Attach(customer);

            customer.FirstName = newCustomer.FirstName;
            customer.LastName = newCustomer.LastName;
            customer.DateOfBirth = newCustomer.DateOfBirth;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomer(Guid customerId)
        {
            var customer = await GetCustomer(customerId);
            if (customer == default(Customer))
            {
                // Maybe do some logging if this wasn't a coding test?
                return;
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }

        public async Task<Customer> Search(string searchTerm)
        {
            return await _context.Customers.SingleOrDefaultAsync(c =>
                c.FirstName.Contains(searchTerm) || c.LastName.Contains(searchTerm)
            );
        }
    }
}
