using Microsoft.AspNetCore.Mvc;

using Customer = InventoryManagement.Contracts.Customer;

namespace InventoryManagement.Services
{
    public interface ICustomerService
    {
        public Task<ServiceResponse<List<Customer>>> Retrieve();

        public  Task<ServiceResponse<Customer>> RetrieveById(int id);

        public Task<ServiceResponse<List<Customer>>> Create(Customer Item);

        public Task<ServiceResponse<Customer>> Update(Customer request);

        public Task<ServiceResponse<List<Customer>>> DeleteSpecific(int id);
    }
}
