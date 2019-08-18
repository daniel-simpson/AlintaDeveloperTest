using System;
using System.Threading.Tasks;
using Alinta.Customers.Models;

namespace Alinta.Customers.Services
{
    public class CustomerService : ICustomerService
    {
        public Task AddCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task EditCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> GetCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public Task<Customer> Search(string searchTerm)
        {
            throw new NotImplementedException();
        }
    }
}
