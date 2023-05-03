using System.ComponentModel.DataAnnotations;
using InventoryManagement.Common;

namespace InventoryManagement.Contracts
{ 
    public class Customer
    {
        public int? id { get; set; }
        public string? firstName { get; set; } = string.Empty;

        public string? lastName { get; set; } = string.Empty;

        public string? address { get; set; } = string.Empty;

        [RegularExpression(@"^(?:^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$|)$",
            ErrorMessage = "Please enter a valid EmailId")]
        public string? email { get; set; } = string.Empty;
       
        [RegularExpression(@"^$|^[0-9]*$",
            ErrorMessage = "Please enter a valid Number")]
        public string? contactNumber { get; set; } = string.Empty; //

        /*
        public User? user { get; set; } //Navigation Property, If planning to use authentication for Customer use this,authentication might cause issues in crud using injections
        public int? userId { get; set; }//Foreign key if planning to use authentication
        public string firstName { get; set; } = string.Empty;
        public string address { get; set; } = string.Empty;
        */
    }
}
