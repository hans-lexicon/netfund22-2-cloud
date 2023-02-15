using TemperatureSensor.Models;
using TemperatureSensor.Services;

/* Starting Device */
var deviceSettings = new DeviceSettings();
await DeviceService.StartUpAsync();

/* Register Device to Azure IoT Hub */
DeviceService.ApiUrl = "http://localhost:7078/api/RegisterDevice";
await DeviceService.RegisterDeviceAsync(deviceSettings);

/* Initializing Device */
await DeviceService.InitializeDeviceAsync(deviceSettings);

/* Send Telemetry every 10 sec */
await DeviceService.SendTelemetryDataAsync(deviceSettings);

Console.ReadKey();