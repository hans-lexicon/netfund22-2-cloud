using System.ComponentModel.DataAnnotations;


namespace AzureFunctions.Models
{
    public class TelemetryEntity
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Data { get; set; } = null!;
        public string? SystemProperties { get; set; }
        public string PartitionKey { get; set; } = "Telemetry";
    }
}
