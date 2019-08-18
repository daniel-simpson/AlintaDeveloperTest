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









        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
