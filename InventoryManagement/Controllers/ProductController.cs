using InventoryManagement.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Product = InventoryManagement.Contracts.Product;


namespace SomeAPI.Controllers
{
    [Route("InventoryManagement/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> Retrieve()
        {
            var products = await _productService.Retrieve();
            return products;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Product>>> RetrieveById(int id)
        {
            var products = await _productService.RetrieveById(id);
            return Ok(products);
        }

        [HttpPost, Authorize(Roles="Admin")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> Create(List<Product> Items)
        {
            var products = await _productService.Create(Items);
            return Ok(products);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> Update(Product request)
        {
            var products = await _productService.Update(request);
            return Ok(products);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> DeleteSpecific(int id)
        {
            var products = await _productService.DeleteSpecific(id);
            return Ok(products);
        }
    }
}