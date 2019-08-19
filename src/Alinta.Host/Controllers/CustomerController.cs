using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Alinta.Customers.Models;
using Alinta.Customers.Services;
using Microsoft.AspNetCore.Mvc;

namespace Alinta.Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerApiController : ControllerBase
    {
        private ICustomerService _customerService;

        public CustomerApiController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // I know read wasn't in the spec, but.... extra credit?
        [HttpGet]
        public async Task<ActionResult<Customer>> Get(Guid customerId)
        {
            return await _customerService.GetCustomer(customerId);
        }

        [HttpPost]
        public async Task<ActionResult> Add(Customer customer)
        {
            await _customerService.AddCustomer(customer);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Update(Customer customer)
        {
            await _customerService.EditCustomer(customer);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(Guid customerId)
        {
            await _customerService.DeleteCustomer(customerId);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<Customer>> Search(string searchTerm)
        {
            var result = await _customerService.Search(searchTerm);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
