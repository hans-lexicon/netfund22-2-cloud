using Microsoft.Azure.Devices.Client;

namespace TemperatureSensor.Models
{
    internal class DeviceSettings
    {
        public DeviceClient DeviceInstance { get; set; } = null!;
        public string DeviceId { get; set; } = "tempsensor";
        public string DeviceType { get; set; } = "Temperature Sensor";
        public string ConnectionString { get; set; } = string.Empty;
        public bool AllowSending { get; set; } = false;
    }
}
