using InventoryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Customer = InventoryManagement.Contracts.Customer;


namespace SomeAPI.Controllers
{
    [Route("InventoryManagement/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
          _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Customer>>>> Retrieve()
        {
            var customers = await _customerService.Retrieve();
            return customers;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Customer>>> RetrieveById(int id)
        {
            var customers = await _customerService.RetrieveById(id);
            return Ok(customers);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<Customer>>>> Create(List<Customer> Items)
        {
            var customers = await _customerService.Create(Items);
            return Ok(customers);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<Customer>>>> Update(Customer request)
        {
            var customers = await _customerService.Update(request);
            return Ok(customers);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<Customer>>>> DeleteSpecific(int id)
        {
            var customers = await _customerService.DeleteSpecific(id);
            return Ok(customers);
        }
    }
}
