namespace SmartHome.Models
{
    public class DeviceTile
    {
        public string DeviceId { get; set; } = null!;
        public string CommonName { get; set; } = null!;
        public string? DeviceType { get; set; }
        public bool IsEnabled { get; set; }
    }
}
