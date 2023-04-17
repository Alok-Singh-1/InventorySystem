using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.EntityFrameworkCore
{
    [Table("Product")]
    public class Product
    {
        public int? id { get; set; }
        public string productCategory { get; set; }

        public string productName { get; set; } 

        public string? description { get; set; } 

        public int productQuantity { get; set; }

        public int sellingPrice { get; set; } //keep in purchase table
    }
}
