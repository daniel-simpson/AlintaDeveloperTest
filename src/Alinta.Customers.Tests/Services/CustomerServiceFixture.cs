using Alinta.Customers.Data;
using Alinta.Customers.Models;
using Alinta.Customers.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Alinta.Customers.Tests.Services
{
    public class CustomerServiceFixture
    {
        private readonly Guid _testRecordGuid;
        private readonly CustomerDataContext _testDataContext;
        private readonly ICustomerService _testCustomerService;

        public CustomerServiceFixture()
        {
            // Set up in memory db options scoped to current test fixture
            DbContextOptions<CustomerDataContext> options;
            var builder = new DbContextOptionsBuilder<CustomerDataContext>();
            builder.UseInMemoryDatabase(nameof(CustomerServiceFixture));
            options = builder.Options;

            // Create customer db
            _testDataContext = new CustomerDataContext(options);
            _testDataContext.Database.EnsureDeleted();
            _testDataContext.Database.EnsureCreated();

            //Seed with one test record
            _testRecordGuid = Guid.NewGuid();
            _testDataContext.Add(new Customer
            {
                Id = _testRecordGuid,
                FirstName = "Daniel",
                LastName = "Simpson",
                DateOfBirth = new DateTime(1988, 1, 5)
            });
            _testDataContext.SaveChanges();

            //Create test CustomerService
            _testCustomerService = new CustomerService(_testDataContext);
        }

        [Fact]
        public async Task GetCustomer_ReturnsNull_WhenCustomerNotFound()
        {
            var returnedCustomer = await _testCustomerService.GetCustomer(Guid.Empty);
            returnedCustomer.Should().BeNull();
        }

        [Fact]
        public async Task GetCustomer_ReturnsValidCustomer_WhenDataExists()
        {
            var returnedCustomer = await _testCustomerService.GetCustomer(_testRecordGuid);
            returnedCustomer.Should().NotBeNull();
        }

        [Fact]
        public async Task AddCustomer_CanAddACustomerToTheDb()
        {
            var newCustomer = new Customer
            {
                Id = Guid.NewGuid(),
                FirstName = "Harry",
                LastName = "Tasker",
                DateOfBirth = new DateTime(1970, 2, 2)
            };

            var customersBeforeOperation = await _testDataContext.Customers.CountAsync();

            await _testCustomerService.AddCustomer(newCustomer);

            var customersAfterOperation = await _testDataContext.Customers.CountAsync();

            customersAfterOperation.Should().Be(customersBeforeOperation + 1);
        }

        [Fact]
        public async Task EditCustomer_CanEditData()
        {
            var updatedCustomer = new Customer
            {
                Id = _testRecordGuid,
                FirstName = "Daniel",
                LastName = "Simps",
                DateOfBirth = new DateTime(1980, 5, 1)
            };

            await _testCustomerService.EditCustomer(updatedCustomer);

            //Also implicitly checks that there is only one record
            var updatedData = await _testDataContext.Customers.SingleAsync();

            updatedData.Should().NotBeNull();
            updatedData.FirstName.Should().Be(updatedCustomer.FirstName);
            updatedData.LastName.Should().Be(updatedCustomer.LastName);
            updatedData.DateOfBirth.Should().Be(updatedCustomer.DateOfBirth);
        }

        [Fact]
        public async Task DeleteCustomer_CanDeleteCustomer()
        {
            await _testCustomerService.DeleteCustomer(_testRecordGuid);

            var count = await _testDataContext.Customers.CountAsync();
            count.Should().Be(0);
        }

        [Theory]
        [InlineData("Ben")]
        [InlineData("Alinta")]
        public async Task SearchCustomer_ReturnsNullIfNoCustomerMatchesQuery(string searchTerm)
        {
            var result = await _testCustomerService.Search(searchTerm);
            result.Should().BeNull();
        }

        [Theory]
        [InlineData("Dan")]
        [InlineData("Sim")]
        [InlineData("Simpson")]
        public async Task SearchCustomer_CanFindByPartialName(string searchTerm)
        {
            var result = await _testCustomerService.Search(searchTerm);
            result.Should().NotBeNull();
        }
    }
}
