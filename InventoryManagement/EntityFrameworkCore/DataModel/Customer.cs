﻿//using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InventoryManagement.Common;

namespace InventoryManagement.EntityFrameworkCore
{
    [Table("Customer")]
    public class Customer
    {
            public int id { get; set; }
            public string firstName { get; set; } = string.Empty;

            public string? lastName { get; set; } = string.Empty;

            public string address { get; set; } = string.Empty;

            /*[RegularExpression(@"^(?:^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$|)$",
                ErrorMessage = "EnterValidEmailAddress")]*/
            public string? email { get; set; } = string.Empty;

            public string contactNumber { get; set; } = string.Empty;

            public PaymentMethod paymentType { get; set; } = PaymentMethod.COD;


    }
}
