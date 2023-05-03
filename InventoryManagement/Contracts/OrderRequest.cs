using System.Diagnostics.Contracts;
using System.Net;

namespace InventoryManagement.Contracts
{
    public class OrderRequest
    {
        public int? id { get; set; }

        public Customer Customer { get; set; }

        public List<OrderItem> OrderItems { get; set; }

    }
}