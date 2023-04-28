using InventoryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderResponse = InventoryManagement.Contracts.OrderResponse;
using OrderRequest= InventoryManagement.Contracts.OrderRequest;


namespace SomeAPI.Controllers
{
    [Route("InventoryManagement/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderservice;
        public OrderController(IOrderService orderservice)
        {
            _orderservice = orderservice;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<OrderResponse>>> RetrieveOrderById(int id)
        {
            var orders = await _orderservice.RetrieveById(id);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<OrderResponse>>> Purchase(OrderRequest Item)
        {
            var orders = await _orderservice.Create(Item);
            return Ok(orders);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<OrderResponse>>>> EditOrder(OrderRequest request)
        {
            var orders = await _orderservice.Update(request);
            return Ok(orders);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<OrderResponse>>>> DeleteOrder(int id)
        {
            var orders = await _orderservice.DeleteSpecific(id);
            return Ok(orders);
        }
    }
}