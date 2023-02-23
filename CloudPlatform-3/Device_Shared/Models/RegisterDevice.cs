namespace Device_Shared.Models
{
    public class RegisterDevice
    {
        public string DeviceId { get; set; } = Guid.NewGuid().ToString();
        public string DeviceType { get; set; } = null!;
        public string CommonName { get; set; } = null!;
        public bool IsEnabled { get; set; } = false;
        public string ConnectionString { get; set; } = null!;
    }
}
