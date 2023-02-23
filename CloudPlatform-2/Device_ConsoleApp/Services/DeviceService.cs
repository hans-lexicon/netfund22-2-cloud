using Device_ConsoleApp.Contexts;
using Device_ConsoleApp.Models;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Device_ConsoleApp.Services
{
    internal class DeviceService
    {
        private readonly DataContext _context = new DataContext();
        private DeviceEntity? _deviceEntity;
        private DeviceClient? _deviceClient;

        public bool IsConfigured { get; private set; } 
        public bool AllowSending { get; private set; }

        public virtual async Task InitializeAsync(string? deviceId = null, string? deviceType = null)
        {
            Console.WriteLine("Initializing device. Please wait...");

            _deviceEntity = await _context.Device.FirstOrDefaultAsync();
            if (_deviceEntity == null)
            {
                _deviceEntity = new DeviceEntity 
                { 
                    Id = !string.IsNullOrEmpty(deviceId) ? deviceId : Guid.NewGuid().ToString(),
                    DeviceType = !string.IsNullOrEmpty(deviceType) ? deviceType : null!
                };

                _context.Add(_deviceEntity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RegisterAsync(string url)
        {
            Console.WriteLine("Registering device. Please wait...");

            if (string.IsNullOrEmpty(_deviceEntity?.ConnectionString))
            {
                using var http = new HttpClient();

                while (!IsConfigured)
                {
                    
                    try
                    {
                        var response = await http.PostAsync(url, new StringContent(_deviceEntity!.Id));
                        if (response.IsSuccessStatusCode)
                        {
                            _deviceEntity.ConnectionString = await response.Content.ReadAsStringAsync();
                            _context.Update(_deviceEntity);
                            await _context.SaveChangesAsync();
                            
                            IsConfigured = true;
                        }
                    }
                    catch
                    {
                        await Task.Delay(5000);
                    }


                }
            } else
            {
                IsConfigured = true;
            }

            _deviceClient = DeviceClient.CreateFromConnectionString(_deviceEntity?.ConnectionString);
            
            AllowSending = true;
        }

        public async Task UpdateTwinAsync()
        {
            if (!string.IsNullOrEmpty(_deviceEntity?.DeviceType))
            {
                Console.WriteLine("Updating device twin. Please wait...");

                if (_deviceClient != null)
                {
                    var twin = await _deviceClient.GetTwinAsync();
                    if (twin != null)
                    {
                        var twinCollection = new TwinCollection();
                        twinCollection["deviceType"] = _deviceEntity.DeviceType;
                        await _deviceClient.UpdateReportedPropertiesAsync(twinCollection);
                    }
                }
            }

        }

        public async Task SendTelemetryAsync(object payload)
        {
            if(_deviceClient != null)
                await _deviceClient.SendEventAsync(new Message(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload))));

            Console.WriteLine($"Telemetry data sent at {DateTime.Now}");
        }
    }
}
