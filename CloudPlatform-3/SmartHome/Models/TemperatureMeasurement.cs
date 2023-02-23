namespace SmartHome.Models
{
    public class TemperatureMeasurement
    {
        public int CurrentTemperature { get; set; }
        public int MinValue { get; set; } = 16;
        public int MaxValue { get; set; } = 30;
    }
}
