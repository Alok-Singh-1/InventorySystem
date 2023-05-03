using System.Net;

namespace InventoryManagement.Contracts
{
    public class OrderItemResponse
    {
        public Customer? Customer { get; set; }

        public List<OrderResponseItem>? OrderItems { get; set; }

    }
}