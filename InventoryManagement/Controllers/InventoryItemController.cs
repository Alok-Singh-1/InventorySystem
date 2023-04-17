using InventoryManagement.Services;
using Microsoft.AspNetCore.Mvc;
using InventoryItems = InventoryManagement.Contracts.InventoryItems;


namespace SomeAPI.Controllers
{
    [Route("InventoryManagement/[controller]")]
    [ApiController]
    public class InventoryItemController : ControllerBase
    {
        private readonly IInventoryService _inventoryservice;
        public InventoryItemController(IInventoryService inventoryservice)
        {
            _inventoryservice = inventoryservice;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<InventoryItems>>>> Retrieve()
        {
            var items = await _inventoryservice.Retrieve();
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<InventoryItems>>> RetrieveById(int id)
        {
            var items = await _inventoryservice.RetrieveById(id);
            return Ok(items);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<InventoryItems>>>> Create(List<InventoryItems> Items)
        {
            var items = await _inventoryservice.Create(Items);
            return Ok(items);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<InventoryItems>>>> Update(InventoryItems request)
        {
            var items = await _inventoryservice.Update(request);
            return Ok(items);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<InventoryItems>>>> DeleteSpecific(int id)
        {
            var items = await _inventoryservice.DeleteSpecific(id);
            return Ok(items);
        }
    }
}