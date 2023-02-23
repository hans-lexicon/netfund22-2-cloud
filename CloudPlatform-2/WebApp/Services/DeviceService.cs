using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;


namespace WebApp.Services
{
    public class DeviceService
    {
        private readonly RegistryManager _registryManager = RegistryManager.CreateFromConnectionString("HostName=netfund22-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=kPhzbwpJqC3Zw+pNm4NbTBbgde5DRoU5aYIiCoDOY9g=");
    
        public async Task<IEnumerable<Twin>> GetDevicesAsync()
        {
            var devices = new List<Twin>();

            var query = _registryManager.CreateQuery("select * from devices");
            while (query.HasMoreResults)
            {
                foreach (var device in await query.GetNextAsTwinAsync())
                {
                    devices.Add(device);
                }
            }

            return devices;
        }


        public async Task<IEnumerable<Twin>> GetDevicesFromHttpAsync()
        {
            using var http = new HttpClient();
            var result = await http.GetFromJsonAsync<IEnumerable<Twin>>("http://localhost:7191/api/devices");
            return result!;
        }
    }
}
