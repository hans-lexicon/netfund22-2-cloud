using SmartHome.Models;

namespace SmartHome.ViewModels.Home
{
    public class IndexViewModel
    {
        public int DeviceCount { get; set; }
        public TemperatureMeasurement TemperatureMeasurement { get; set; } = new();
        public IEnumerable<DeviceTile> Devices { get; set; } = new List<DeviceTile>();
    }
}
