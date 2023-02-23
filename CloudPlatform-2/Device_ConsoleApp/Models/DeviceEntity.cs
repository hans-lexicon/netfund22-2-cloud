using System.ComponentModel.DataAnnotations;

namespace Device_ConsoleApp.Models
{
    internal class DeviceEntity
    {
        [Key]
        public string Id { get; set; } = null!;
        public string? DeviceType { get; set; }
        public string? ConnectionString { get; set; }
    }
}
