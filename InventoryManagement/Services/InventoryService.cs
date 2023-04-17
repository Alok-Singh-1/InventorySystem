using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using InventoryItems=InventoryManagement.Contracts.InventoryItems;

namespace InventoryManagement.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IMapper _mapper;
        private readonly InventoryContext _context;
        public InventoryService(IMapper mapper, InventoryContext context)
        {
            _mapper=mapper;
            _context=context;
        }
        public async Task<ServiceResponse<List<InventoryItems>>> Retrieve()
        {
            var serviceResponse = new ServiceResponse<List<InventoryItems>>();
            var dbCustomers=await _context.InventoryItems.ToListAsync();


            var items = new List<Contracts.InventoryItems>();


            foreach (var item in dbCustomers.Select(item => _mapper.Map<Contracts.InventoryItems>(item)))
            {
                if (item is null)
                {
                    throw new Exception($"No InventoryItems Exists");
                }
                else
                {
                    items.Add(item);
                }
            }
            // _mapper.Map<EntityFrameworkCore.InventoryItems,Contracts.InventoryItems >(dbCustomers);
            serviceResponse.Data = items;
            return serviceResponse;
        }

        public async Task<ServiceResponse<InventoryItems>> RetrieveById(int id)
        {
            var serviceResponse = new ServiceResponse<InventoryItems>();
            var item = await _context.InventoryItems.FirstOrDefaultAsync(x => x.id == id);//            var api = _someApi.Find(x => x.id == id);
            if (item == null) throw new Exception("Id not valid"); //null doesn't work in this case
            serviceResponse.Data = _mapper.Map<Contracts.InventoryItems>(item);
            return serviceResponse;
        }
        public async Task<ServiceResponse<List<InventoryItems>>> Create(List<InventoryItems> Items)
        {
            var serviceResponse = new ServiceResponse<List<InventoryItems>>();
            var dbCustomer = new List<EntityFrameworkCore.InventoryItems>();
            
            foreach (var item in Items.Select(code => _mapper.Map<EntityFrameworkCore.InventoryItems>(code)))
            {
                dbCustomer.Add(item);
            }
            _context.InventoryItems.AddRange(dbCustomer);
            await _context.SaveChangesAsync();

            var updatedList=await _context.InventoryItems.ToListAsync(); //return Ok here if latest list is not required to be updated
            var outputList = updatedList.Select(code => _mapper.Map<Contracts.InventoryItems>(code)).ToList();
            serviceResponse.Data = outputList;
            return serviceResponse;
        }

        public async Task<ServiceResponse<InventoryItems>> Update(InventoryItems request)
        {

            var serviceResponse = new ServiceResponse<InventoryItems>();

            try
            {
                var item = await _context.InventoryItems.FirstOrDefaultAsync(x => x.id == request.id);
                if (item is null) 
                    throw new Exception($"InventoryItems with ID'{request.id}' not found.");
                item.productName = request.productName;
                item.purchasePrice = request.purchasePrice;
                item.quantity = request.quantity;
                item.productCategory = request.productCategory;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<Contracts.InventoryItems>(item);
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<InventoryItems>>> DeleteSpecific(int id)
        {
            var serviceResponse = new ServiceResponse<List<InventoryItems>>();

            try
            {
                var item = await _context.InventoryItems.FirstOrDefaultAsync(x => x.id == id);
                if (item is null)
                    throw new Exception($"InventoryItems with ID '{id}' not found.");

                _context.InventoryItems.Remove(item);

                await _context.SaveChangesAsync();


                var updatedList = await _context.InventoryItems.ToListAsync(); 
                var outputList = updatedList.Select(code => _mapper.Map<Contracts.InventoryItems>(code)).ToList();
                serviceResponse.Data = outputList;

            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;

        }
    }
}
