using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Product=InventoryManagement.Contracts.Product;

namespace InventoryManagement.Services
{
    public class ProductService : IProductService
    {
        private readonly IMapper _mapper;
        private readonly InventoryContext _context;
        public ProductService(IMapper mapper, InventoryContext context)
        {
            _mapper=mapper;
            _context=context;
        }
        public async Task<ServiceResponse<List<Product>>> Retrieve()
        {
            var serviceResponse = new ServiceResponse<List<Product>>();
            var dbProducts=await _context.Products.ToListAsync();


            var products = new List<Contracts.Product>();


            foreach (var product in dbProducts.Select(item => _mapper.Map<Contracts.Product>(item)))
            {
                if (product is null)
                {
                    throw new Exception($"No Products Exists");
                }
                else
                {
                    products.Add(product);
                }
            }
            // _mapper.Map<EntityFrameworkCore.Product,Contracts.Product >(dbCustomers);
            serviceResponse.Data = products;
            return serviceResponse;
        }

        public async Task<ServiceResponse<Product>> RetrieveById(int id)
        {
            var serviceResponse = new ServiceResponse<Product>();
            var product = await _context.Products.FirstOrDefaultAsync(x => x.id == id);//            var api = _someApi.Find(x => x.id == id);
            if (product == null)
            {
                serviceResponse.Message = "Product Not Found"; //null doesn't work in this case
                serviceResponse.Success = false;
                return serviceResponse;
            }
            serviceResponse.Data = _mapper.Map<Contracts.Product>(product);
            return serviceResponse;
        }
        public async Task<ServiceResponse<List<Product>>> Create(List<Product> Items)
        {
            var serviceResponse = new ServiceResponse<List<Product>>();
            var dbCustomer = new List<EntityFrameworkCore.Product>();
            
            foreach (var item in Items.Select(code => _mapper.Map<EntityFrameworkCore.Product>(code)))
            {
                dbCustomer.Add(item);
            }
            _context.Products.AddRange(dbCustomer);
            await _context.SaveChangesAsync();

            var updatedList=await _context.Products.ToListAsync(); //return Ok here if latest list is not required to be updated
            var outputList = updatedList.Select(code => _mapper.Map<Contracts.Product>(code)).ToList();
            serviceResponse.Data = outputList;
            return serviceResponse;
        }

        public async Task<ServiceResponse<Product>> Update(Product request)
        {
            var serviceResponse = new ServiceResponse<Product>();

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.id == request.id);
                if (product is null) 
                    throw new Exception($"Product with ID'{request.id}' not found.");
                product.productName = request.productName;
                product.productQuantity = request.productQuantity;
                product.sellingPrice = request.sellingPrice;
                product.description = request.description;
                product.productCategory = request.productCategory;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<Contracts.Product>(product);
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Product>>> DeleteSpecific(int id)
        {
            var serviceResponse = new ServiceResponse<List<Product>>();

            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(x => x.id == id);
                if (product is null)
                    throw new Exception($"Product with ID '{id}' not found.");

                _context.Products.Remove(product);

                await _context.SaveChangesAsync();


                var updatedList = await _context.Products.ToListAsync(); 
                var outputList = updatedList.Select(code => _mapper.Map<Contracts.Product>(code)).ToList();
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
