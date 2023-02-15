using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;
using System.Text;
using TemperatureSensor.Models;

namespace TemperatureSensor.Services
{
    internal class DeviceService
    {
        public static string ApiUrl { get; set; } = "";


        public static async Task StartUpAsync()
        {
            Console.WriteLine("Starting up...");
            await Task.Delay(10000);
        }

        public static async Task RegisterDeviceAsync(DeviceSettings deviceSettings)
        {

            Console.WriteLine("Registering Device with Azure IoT Hub...");
            using var http = new HttpClient();
            var registered = false;

            while (!registered) 
            {
                try
                {
                    var response = await http.PostAsync(ApiUrl, new StringContent(JsonConvert.SerializeObject(deviceSettings)));
                    var result = await response.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(result))
                    {
                        deviceSettings.ConnectionString = result;
                        registered = true;
                    }
                }
                catch 
                { 
                    await Task.Delay(10 * 1000);
                    Console.WriteLine("Retrying to Registering Device with Azure IoT Hub...");
                }
            }

        }

        public static async Task InitializeDeviceAsync(DeviceSettings deviceSettings)
        {
            if (!string.IsNullOrEmpty(deviceSettings.ConnectionString))
            {

                Console.WriteLine("Initializing Device...");
                deviceSettings.DeviceInstance = DeviceClient.CreateFromConnectionString(deviceSettings.ConnectionString);

                var twinCollection = new TwinCollection();
                twinCollection["deviceType"] = deviceSettings.DeviceType;
                await deviceSettings.DeviceInstance.UpdateReportedPropertiesAsync(twinCollection);

                deviceSettings.AllowSending = true;
            }
        }

        public static async Task SendTelemetryDataAsync(DeviceSettings deviceSettings)
        {
            Console.WriteLine("Sending telemetry data to Azure IoT Hub...");
            while (deviceSettings.AllowSending)
            {
                var message = JsonConvert.SerializeObject(new { temperature = 23, humidity = 44, time = DateTime.Now.ToString() });
                await deviceSettings.DeviceInstance.SendEventAsync(new Message(Encoding.UTF8.GetBytes(message)));
                await Task.Delay(10000);
            }
        }
    }
}
