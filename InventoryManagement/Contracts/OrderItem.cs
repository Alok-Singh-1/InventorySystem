using System.Net;

namespace InventoryManagement.Contracts
{
    public class OrderItem
    {
        public int? id { get; set; }

        public int productId { get; set; }

        public int quantity { get; set; }

        //public List<>
    }
}