using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.EntityFrameworkCore
{
    [Table("Order")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int quantity { get; set; }

        public int total_price { get; set; }

        public string transactionStatus { get; set; } 

        public DateTime? orderDate { get; set; }

        public Decimal? Discount { get; set; }

        public Customer customer { get; set; } //navigationalProperty

        public int customerId { get; set; }

        public Product product { get; set; } //navigationalProperty

        public int productId { get; set; }  //Product Id is a foreign key relating the order to the specific entity of the product table
    }
}
