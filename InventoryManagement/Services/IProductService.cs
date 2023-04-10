using Microsoft.AspNetCore.Mvc;

using Product = InventoryManagement.Contracts.Product;

namespace InventoryManagement.Services
{
    public interface IProductService
    {
        public Task<ServiceResponse<List<Product>>> Retrieve();

        public  Task<ServiceResponse<Product>> RetrieveById(int id);

        public Task<ServiceResponse<List<Product>>> Create(List<Product> Items);

        public Task<ServiceResponse<Product>> Update(Product request);

        public Task<ServiceResponse<List<Product>>> DeleteSpecific(int id);
    }
}
