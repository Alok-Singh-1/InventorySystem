using AutoMapper;
using Npgsql;
using Customer = InventoryManagement.Contracts.Customer;

namespace InventoryManagement.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly InventoryContext _context;
        public CustomerService(IMapper mapper, InventoryContext context)
        {
            _mapper=mapper;
            _context=context;
        }
        public async Task<ServiceResponse<List<Customer>>> Retrieve()
        {
            var serviceResponse = new ServiceResponse<List<Customer>>(new List<Customer>());
            var dbCustomers=await _context.Customers.ToListAsync();


            var customers = new List<Contracts.Customer>();


            foreach (var customer in dbCustomers.Select(item => _mapper.Map<Contracts.Customer>(item)))
            {
                if (customer is null)
                {
                    throw new Exception($"No Customers Exists");
                }
                else
                {
                    customers.Add(customer);
                }
            }
            // _mapper.Map<EntityFrameworkCore.Customer,Contracts.Customer >(dbCustomers);
            serviceResponse.Data = customers;
            return serviceResponse;
        }

        public async Task<ServiceResponse<Customer>> RetrieveById(int id)
        {
            var serviceResponse = new ServiceResponse<Customer>(new Customer());
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.id == id);//            var api = _someApi.Find(x => x.id == id);
            if (customer == null) throw new Exception("Customer id not valid"); //null doesn't work in this case
            serviceResponse.Data = _mapper.Map<Contracts.Customer>(customer);
            return serviceResponse;
        }
        public async Task<ServiceResponse<List<Customer>>> Create(Customer Item)
        {
            var serviceResponse = new ServiceResponse<List<Customer>>(new List<Customer>());
            var dbCustomer = new List<EntityFrameworkCore.Customer>();

            dbCustomer.Add(_mapper.Map<EntityFrameworkCore.Customer>(Item));
            
            /*foreach (var item in Items.Select(code => _mapper.Map<EntityFrameworkCore.Customer>(code))) //should be used if input parameter is List<Customer> type right now its just Customer
            {
                dbCustomer.Add(item);
            }*/
            _context.Customers.AddRange(dbCustomer);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {

                bool uniqueViolation = ((Npgsql.PostgresException)ex.InnerException).Code=="23505";
                bool isContactDuplicate=((Npgsql.PostgresException)ex.InnerException).ConstraintName.Contains("contactNumber");
                if (uniqueViolation && isContactDuplicate)
                {
                    serviceResponse.Success = false;
                    serviceResponse.Message = "Contact number already exists, please enter a new number";
                    return serviceResponse;
                }
            }

            var updatedList=await _context.Customers.ToListAsync(); //return Ok here if latest list is not required to be updated
            var outputList = updatedList.Select(code => _mapper.Map<Contracts.Customer>(code)).ToList();
            serviceResponse.Data = outputList;
            return serviceResponse;
        }

        public async Task<ServiceResponse<Customer>> Update(Customer request)
        {

            var serviceResponse = new ServiceResponse<Customer>(new Customer());

            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(x => x.id == request.id);
                if (customer is null) 
                    throw new Exception($"Customer with ID'{request.id}' not found.");
                customer.firstName = request.firstName;
                customer.lastName = request.lastName;
                customer.address = request.address;
                customer.contactNumber = request.contactNumber;
                customer.email = request.email;

                await _context.SaveChangesAsync();

                serviceResponse.Data = _mapper.Map<Contracts.Customer>(customer);
            }
            catch(Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<ServiceResponse<List<Customer>>> DeleteSpecific(int id)
        {
            var serviceResponse = new ServiceResponse<List<Customer>>(new List<Customer>());

            try
            {
                var customer = await _context.Customers.FirstOrDefaultAsync(x => x.id == id);
                if (customer is null)
                    throw new Exception($"Customer with ID '{id}' not found.");

                _context.Customers.Remove(customer);

                await _context.SaveChangesAsync();


                var updatedList = await _context.Customers.ToListAsync(); 
                var outputList = updatedList.Select(code => _mapper.Map<Contracts.Customer>(code)).ToList();
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
