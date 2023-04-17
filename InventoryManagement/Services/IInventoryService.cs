using Microsoft.AspNetCore.Mvc;

using InventoryItems = InventoryManagement.Contracts.InventoryItems;

namespace InventoryManagement.Services
{
    public interface IInventoryService
    {
        public Task<ServiceResponse<List<InventoryItems>>> Retrieve();

        public  Task<ServiceResponse<InventoryItems>> RetrieveById(int id);

        public Task<ServiceResponse<List<InventoryItems>>> Create(List<InventoryItems> Items);

        public Task<ServiceResponse<InventoryItems>> Update(InventoryItems request);

        public Task<ServiceResponse<List<InventoryItems>>> DeleteSpecific(int id);
    }
}
