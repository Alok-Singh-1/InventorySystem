using System.Net;

namespace InventoryManagement.Contracts
{
    public class OrderResponseItem
    {
        public int? id { get; set; }

        public int productId { get; set; }

        public int quantity { get; set; }

        public int total_price { get; set; }

        public string transactionStatus { get; set; }

        public DateTime? orderDate { get; set; }

      //  public Decimal? Discount { get; set; }
    }
}