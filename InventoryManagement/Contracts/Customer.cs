using InventoryManagement.Common;

namespace InventoryManagement.Contracts
{ 
    public class Customer
    {
        public int? id { get; set; }
        public string firstName { get; set; } = string.Empty;

        public string? lastName { get; set; } = string.Empty;

        public string address { get; set; } = string.Empty;

        public string? email { get; set; } = string.Empty;

        public string contactNumber { get; set; } = string.Empty;
        /*public int? id { get; set; }
        public string firstName { get; set; } = string.Empty;

        public string? lastName { get; set; } = string.Empty;

        public string address { get; set; } = string.Empty;

        public string? email { get; set; } = string.Empty;

        public string? contactNumber { get; set; } = string.Empty;*/

    }
}
