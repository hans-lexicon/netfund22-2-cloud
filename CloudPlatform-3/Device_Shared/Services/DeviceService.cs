using Device_Shared.Models;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System.Security.AccessControl;

namespace Device_Shared.Services
{
    public class DeviceService
    {
        private DeviceClient _deviceClient = null!;
        private FileService _fileService = new FileService(@$"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}\configuration.json");


        public RegisterDevice CreateDevice(string commonName, string deviceType)
        {
            var device = new RegisterDevice();

            try
            {
                device = JsonConvert.DeserializeObject<RegisterDevice>(_fileService.Read());
            }
            catch
            {
                device = new RegisterDevice
                {
                    CommonName = commonName,
                    DeviceType = deviceType
                };

                _fileService.Save(JsonConvert.SerializeObject(device));
            }

            return device!;
        }

        public async Task<string> RegisterDeviceAsync(string deviceId, string registerUrl)
        {
            if (_deviceClient == null)
            {
                using var http = new HttpClient();
                var result = await http.PostAsync(registerUrl, new StringContent(deviceId));
                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStringAsync();
                }
            }

            return string.Empty;
        }

        public async Task InitializeAsync(RegisterDevice device)
        {
            _deviceClient = DeviceClient.CreateFromConnectionString(device.ConnectionString);
            await UpdateTwinAync(device.CommonName, device.DeviceType);
        }

        
        
        public async Task InitializeMethodHandlersAsync()
        {
            Console.WriteLine("Init Method Handlers");
            await _deviceClient.SetMethodHandlerAsync("enable", SetIsEnabled, null);
        }


        private Task<MethodResponse> SetIsEnabled(MethodRequest req, object userContext) 
        {
            Console.WriteLine("SetIsEnabled is triggered");
            UpdateIsEnabledAsync().ConfigureAwait(false);
            return Task.FromResult(new MethodResponse(null, 200));
        }


        private async Task UpdateIsEnabledAsync()
        {
            Console.WriteLine("Updating IsEnabled");
            var twin = await _deviceClient.GetTwinAsync();

            var twinCollection = new TwinCollection();
            twinCollection["isEnabled"] = !twin.Properties.Reported["isEnabled"];

            await _deviceClient.UpdateReportedPropertiesAsync(twinCollection);
        }


        private async Task UpdateTwinAync(string commonName, string deviceType)
        {
            Console.WriteLine("UpdateTwinAsync is running");

            var twin = await _deviceClient.GetTwinAsync();

            var twinCollection = new TwinCollection();
            twinCollection["commonName"] = commonName;
            twinCollection["deviceType"] = deviceType;
            twinCollection["isEnabled"] = false;

            await _deviceClient.UpdateReportedPropertiesAsync(twinCollection);
        }

    }
}
