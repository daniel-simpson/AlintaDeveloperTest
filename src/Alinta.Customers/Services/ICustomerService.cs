using Alinta.Customers.Models;
using System;
using System.Threading.Tasks;

namespace Alinta.Customers.Services
{
    public interface ICustomerService
    {
        Task AddCustomer(Customer customer);
        Task<Customer> GetCustomer(Guid customerId); // Because it wouldn't be CRUD without read
        Task EditCustomer(Customer customer);
        Task DeleteCustomer(Guid customerId);
        Task<Customer> Search(string searchTerm);

    }
}
