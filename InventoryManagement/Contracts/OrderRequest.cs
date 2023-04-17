using System.Net;

namespace InventoryManagement.Contracts
{
    public class OrderRequest
    {
        public int? id { get; set; }

        public int? customerId { get; set; }

        public int productId { get; set; }

        public int quantity { get; set; }

        public List<Customer>? Customer { get; set; }

    }
}