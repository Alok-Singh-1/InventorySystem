namespace InventoryManagement.Contracts
{ 
    public class Product
    {
            public int? id { get; set; }
            public string productCategory { get; set; } = string.Empty;

            public string productName { get; set; } = string.Empty;

            public string? description { get; set; } = string.Empty;

            public int productQuantity { get; set; }

            public int sellingPrice { get; set; }

    }
}
