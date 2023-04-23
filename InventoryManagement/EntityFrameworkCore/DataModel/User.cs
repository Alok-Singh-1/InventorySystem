namespace InventoryManagement.EntityFrameworkCore
{
    public class User
    {
        public int id { get; set; }
        public string userName { get; set; } =string.Empty;
        public byte[] passwordHash { get; set; } = new byte[0];
        public byte[] passwordSalt { get; set; } = new byte[0];

    }
}
