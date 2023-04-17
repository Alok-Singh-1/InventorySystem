namespace InventoryManagement.Contracts
{ 
    public class InventoryItems
    {
        public int? id { get; set; }
        public string productName { get; set; } = string.Empty;

        public string? purchasePrice { get; set; } = string.Empty;

        public string quantity { get; set; } = string.Empty;

        public string? productCategory { get; set; } = string.Empty;

        public string vendorName { get; set; } = string.Empty;
    }
}
