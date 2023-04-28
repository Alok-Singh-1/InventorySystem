using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderResponse = InventoryManagement.Contracts.OrderResponse;
using OrderRequest = InventoryManagement.Contracts.OrderRequest;
using InventoryManagement.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using InventoryManagement.Contracts;

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
        public async Task<ServiceResponse<OrderResponse>> Create(OrderRequest Order)
        {
            var serviceResponse = new ServiceResponse<OrderResponse>();
            var entityOrder = new EntityFrameworkCore.Order();
            var listDbOrder = new List<EntityFrameworkCore.Order>();

            if (Order.Customer.id == null && (Order.Customer.firstName.IsNullOrEmpty() || Order.Customer.contactNumber.IsNullOrEmpty() || Order.Customer.address.IsNullOrEmpty())) ////If neither an existing customer id nor details of a new customer are provided then an error is thrown
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Please provide either an existing customer Id or the details of a new customer i.e. FirstName, Contact and Address";
                return serviceResponse;
            }

            if (Order.Customer.id != null && (!Order.Customer.firstName.IsNullOrEmpty() || !Order.Customer.contactNumber.IsNullOrEmpty() || !Order.Customer.address.IsNullOrEmpty())) //If both existing customer id and details of a new customer are provided then an error is thrown
            {
                serviceResponse.Success = false;
                serviceResponse.Message = "Both existing Customer Id and details of new customer are provided,please provide either an existing customer Id or the details of a new customer";
                return serviceResponse;
            }

            if (Order.Customer.id == null && (!Order.Customer.firstName.IsNullOrEmpty() ||
                                              !Order.Customer.contactNumber.IsNullOrEmpty() ||
                                              !Order.Customer.address
                                                  .IsNullOrEmpty())) //only go inside if id is null
            {
                /*List<Contracts.Customer> customer = new List<Contracts.Customer>();
                customer.Add(Order.Customer);*/
                /*var customerList = await _customerService.Create(customer);   //if there is some content in list create a customer*/
                var customerList = await _customerService.Create(Order.Customer);   //if there is some 
                var LastCustomer = customerList.Data.LastOrDefault();
                Order.Customer.id = LastCustomer.id;
            }


            if(Order.Customer.id > 0 && (Order.Customer.firstName.IsNullOrEmpty() || Order.Customer.contactNumber.IsNullOrEmpty() || Order.Customer.address.IsNullOrEmpty())) ////If neither an existing customer id nor details of a new customer are provided then an error is thrown
            {
                try
                {
                    var customerExists = await _customerService.RetrieveById((int)Order.Customer.id);
                    if (customerExists.Data == null || !customerExists.Data.id.HasValue)
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
            foreach (var order in Order.OrderItems)
            {


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
                entityOrder.customerId = (int)Order.Customer.id;
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
            var item = updatedList.LastOrDefault();
           // newList.Add(_mapper.Map<Contracts.OrderResponse>(item));
            //  var outputList = updatedList.Select(code => _mapper.Map<Contracts.OrderResponse>(code)).ToList();
            //  serviceResponse.Data = outputList;
            serviceResponse.Data = _mapper.Map<Contracts.OrderResponse>(item);
            return serviceResponse;
        }

        public async Task<ServiceResponse<OrderResponse>> Update(OrderRequest UpdatedOrder)
        {

            var serviceResponse = new ServiceResponse<OrderResponse>();

            try
            {

                var order = await _context.Orders.FirstOrDefaultAsync(x => x.id == UpdatedOrder.id);
                if (order is null)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = $"Order with ID'{UpdatedOrder.id}' not found.";
                    return serviceResponse;
                    //throw new Exception($"Order with ID'{request.id}' not found.");
                }
                var customerExists = await _customerService.RetrieveById((int)UpdatedOrder.Customer.id);
                if (UpdatedOrder.Customer.id == null || customerExists.Data == null || !customerExists.Data.id.HasValue) //fault scenario return if goes inside this
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Customer Id Invalid";
                    return serviceResponse;
                }

                foreach (var updatedOrder in UpdatedOrder.OrderItems)
                {
                    var productExists = await _productService.RetrieveById(updatedOrder.productId);
                    if (productExists.Data == null || !productExists.Data.id.HasValue)
                    {
                        serviceResponse.Success = false;
                        serviceResponse.Message = "Product with given Id does not exist";
                        return serviceResponse;
                        //throw new Exception("Customer does not exist");
                    }
                    order.productId = updatedOrder.productId; //will  not work if we really send a list of orders to be updated we need a separate contract for Orders
                    order.quantity = updatedOrder.quantity;//will  not work if we really send a list of orders, will get updated each time removing the data for of previous item in list , update needs to only allow one order to be updated for that we need a different kind of contract where orderItem is not a list
                    order.total_price = ((productExists.Data.sellingPrice) * (order.quantity));
                    order.customerId = (int)UpdatedOrder.Customer.id;
                    order.orderDate = DateTime.UtcNow;
                    //need to send datetime in response do not need it in request //create different contract for response

                    order.transactionStatus = "Completed";
                    await _context.SaveChangesAsync();
                }
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
