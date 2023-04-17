using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderResponse = InventoryManagement.Contracts.OrderResponse;
using OrderRequest = InventoryManagement.Contracts.OrderRequest;
namespace InventoryManagement.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly InventoryContext _context;
        private readonly ICustomerService _customerService;
        private readonly IProductService _productService;
        public OrderService(IMapper mapper, InventoryContext context, ICustomerService customerService, IProductService productService)
        {
            _mapper = mapper;
            _context = context;
            _customerService = customerService;
            _productService = productService;
        }

        public async Task<ServiceResponse<OrderResponse>> RetrieveById(int id)
        {
            var serviceResponse = new ServiceResponse<OrderResponse>();
            var order = await _context.Orders.FirstOrDefaultAsync(x => x.id == id);//            var api = _someApi.Find(x => x.id == id);
            if (order == null)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Order id not valid";
                return serviceResponse;
               // throw new Exception(" Order Id not valid");
            }
            serviceResponse.Data = _mapper.Map<Contracts.OrderResponse>(order);
            return serviceResponse;
        }
        public async Task<ServiceResponse<List<OrderResponse>>> Create(List<OrderRequest> Orders)
        {
            var serviceResponse = new ServiceResponse<List<OrderResponse>>();
            var entityOrder = new EntityFrameworkCore.Order();
            var listDbOrder = new List<EntityFrameworkCore.Order>();

            foreach (var order in Orders)
            {

                if (order.customerId == null && (order.Customer == null || order.Customer?.Count == 0)) //fault scenario return if goes inside this
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Please provide either an existing customer Id or the details of a new customer";
                    return serviceResponse;
                }

                if (order.Customer != null && order.Customer.Count > 0)  //check if there is any content in Customer list inside order
                {
                  var customerList= await _customerService.Create(order.Customer);   //if there is some content in list create a customer
                  var LastCustomer=customerList.Data.LastOrDefault();
                  order.customerId = LastCustomer.id;
                } 

                if (order.customerId != null && order.Customer == null || order.Customer?.Count == 0)
                {
                    try
                    {
                        var customerExists = await _customerService.RetrieveById((int)order.customerId);
                        if (customerExists.Data==null || !customerExists.Data.id.HasValue)
                        {
                            serviceResponse.Success = false;
                            serviceResponse.Message = "Customer does not exist";
                            return serviceResponse;
                            //throw new Exception("Customer does not exist");
                        }
                    }
                    catch (Exception ex)
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = ex.Message;
                        return serviceResponse;
                    }
                }
                var productExists = await _productService.RetrieveById(order.productId);
                if (productExists.Data == null || !productExists.Data.id.HasValue)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Product with given Id does not exist";
                    return serviceResponse;
                    //throw new Exception("Customer does not exist");
                }
                if (productExists.Data.productQuantity == 0)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Item out of stock";
                    return serviceResponse;
                }
                if (productExists.Data.productQuantity < order.quantity)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Only {productExists.Data.productQuantity} items in stocks";
                    return serviceResponse;
                }
                entityOrder.customerId = (int)order.customerId;
                entityOrder.orderDate = DateTime.UtcNow;
                entityOrder.productId = order.productId;
                entityOrder.quantity = order.quantity;
                entityOrder.total_price = ((productExists.Data.sellingPrice) * (entityOrder.quantity));
                entityOrder.transactionStatus = "Completed";
                listDbOrder.Add(entityOrder);

                productExists.Data.productQuantity = (productExists.Data.productQuantity-(order.quantity));//update quantity in product table after order
               await _productService.Update(productExists.Data);
            }
         //   var dbOrders = Orders.Select(code => _mapper.Map<EntityFrameworkCore.Order>(code)).ToList();

            //deal with   total_price,  transactionStatus  orderDate,  Discount in entity Framework contract
          //    _context.Orders.AddRange(dbOrders);

            _context.Orders.AddRange(listDbOrder);

            await _context.SaveChangesAsync();

            var updatedList=await _context.Orders.ToListAsync(); //return Ok here if latest list is not required to be updated
            var outputList = updatedList.Select(code => _mapper.Map<Contracts.OrderResponse>(code)).ToList();
            serviceResponse.Data = outputList;
            return serviceResponse;
        }

        public async Task<ServiceResponse<OrderResponse>> Update(OrderRequest request)
        {

            var serviceResponse = new ServiceResponse<OrderResponse>();

            try
            {

                var order = await _context.Orders.FirstOrDefaultAsync(x => x.id == request.id);
                if (order is null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Order with ID'{request.id}' not found.";
                    return serviceResponse;
                    //throw new Exception($"Order with ID'{request.id}' not found.");
                }
                var customerExists = await _customerService.RetrieveById((int)request.customerId);
                if (request.customerId == null || customerExists.Data == null || !customerExists.Data.id.HasValue) //fault scenario return if goes inside this
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Customer Id Invalid";
                    return serviceResponse;
                }
                var productExists = await _productService.RetrieveById(request.productId);
                if (productExists.Data == null || !productExists.Data.id.HasValue)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Product with given Id does not exist";
                    return serviceResponse;
                    //throw new Exception("Customer does not exist");
                }

                order.customerId = (int)request.customerId;
                order.orderDate = DateTime.UtcNow;
                order.productId = request.productId;
                order.quantity = request.quantity;   //need to send datetime in response do not need it in request //create different contract for response
                order.total_price = ((productExists.Data.sellingPrice) * (order.quantity));
                order.transactionStatus = "Completed";

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<Contracts.OrderResponse>(order);
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<OrderResponse>>> DeleteSpecific(int id)
        {
            var serviceResponse = new ServiceResponse<List<OrderResponse>>();

            try
            {
                var order = await _context.Orders.FirstOrDefaultAsync(x => x.id == id);
                if (order is null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Order id not valid";
                    return serviceResponse;
                    //throw new Exception($"Order with ID '{id}' not found.");
                }

                _context.Orders.Remove(order);

                await _context.SaveChangesAsync();


                var updatedList = await _context.Orders.ToListAsync(); 
                var outputList = updatedList.Select(code => _mapper.Map<Contracts.OrderResponse>(code)).ToList();
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
