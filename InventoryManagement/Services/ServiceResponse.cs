namespace InventoryManagement.Services
{
    public class ServiceResponse<T>
    {
        private readonly T _data;
        public ServiceResponse(T data)
        {
            _data=data;
        }
        public T? Data { get; set; }
        public bool Success { get; set; } = true;

        public string? Message { get; set; }
    }
}
