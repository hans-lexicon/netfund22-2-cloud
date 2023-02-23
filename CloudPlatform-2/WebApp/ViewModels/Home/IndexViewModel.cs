using Microsoft.Azure.Devices.Shared;
using System.Collections;

namespace WebApp.ViewModels.Home
{
    public class IndexViewModel
    {
        public IEnumerable<Twin> DevicesFromRegistryManager { get; set; } = null!;
        public IEnumerable<Twin> DevicesFromHttp { get; set; } = null!;
    }
}
