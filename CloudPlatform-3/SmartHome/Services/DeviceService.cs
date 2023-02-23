using Microsoft.Azure.Devices;
using SmartHome.Models;

namespace SmartHome.Services
{
    public class DeviceService
    {
        private readonly RegistryManager _registryManager = RegistryManager.CreateFromConnectionString("HostName=netfund22-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=kPhzbwpJqC3Zw+pNm4NbTBbgde5DRoU5aYIiCoDOY9g=");
        private readonly ServiceClient _serviceClient = ServiceClient.CreateFromConnectionString("HostName=netfund22-iothub.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=kPhzbwpJqC3Zw+pNm4NbTBbgde5DRoU5aYIiCoDOY9g=");


        public async Task<IEnumerable<DeviceTile>> GetAllAsync()
        {
            var list = new List<DeviceTile>();

            var devices = _registryManager.CreateQuery("select * from devices");
            while (devices.HasMoreResults)
            {
                foreach (var device in await devices.GetNextAsTwinAsync())
                {
                    var _device = new DeviceTile() { DeviceId = device.DeviceId };

                    try { _device.CommonName = device.Properties!.Reported["commonName"].ToString(); } catch { _device.CommonName = "Unknown"; }
                    try { _device.DeviceType = device!.Properties!.Reported["deviceType"].ToString(); } catch { }
                    try { _device.IsEnabled = device.Properties!.Reported["isEnabled"].ToString(); } catch { }
                    
                    list.Add(_device);
                }
            }

            return list;
        }

        public async Task<string> SendDirectMethodAsync(string deviceId, string methodName, string? payload = null)
        {
            var method = new CloudToDeviceMethod(methodName);

            if(!string.IsNullOrEmpty(payload))
                method.SetPayloadJson(payload);

            var response = await _serviceClient.InvokeDeviceMethodAsync(deviceId, method);
            if (response.Status == 200)
            {
                try
                {
                    var result = response.GetPayloadAsJson();
                    return result;
                }
                catch { }
            }

            return null!;
        }
    }
}
