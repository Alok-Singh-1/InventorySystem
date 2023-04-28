using Microsoft.AspNetCore.Mvc;
using OrderResponse = InventoryManagement.Contracts.OrderResponse;
using OrderRequest = InventoryManagement.Contracts.OrderRequest;


namespace InventoryManagement.Services
{
    public interface IOrderService
    {

        public  Task<ServiceResponse<OrderResponse>> RetrieveById(int id);

        public Task<ServiceResponse<OrderResponse>> Create(OrderRequest Items);

        public Task<ServiceResponse<OrderResponse>> Update(OrderRequest request);

        public Task<ServiceResponse<List<OrderResponse>>> DeleteSpecific(int id);
    }
}
