using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace InventoryManagement.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PaymentMethod
    { 
        COD=1
    }
}
