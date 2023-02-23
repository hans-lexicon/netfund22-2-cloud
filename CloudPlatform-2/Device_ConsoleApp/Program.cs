using Device_ConsoleApp.Services;
var deviceService = new DeviceService();

while (true)
{
    await deviceService.InitializeAsync();
    await deviceService.RegisterAsync("http://localhost:7191/api/devices");
    await deviceService.UpdateTwinAsync();


    while (deviceService.IsConfigured && deviceService.AllowSending)
    {
        await deviceService.SendTelemetryAsync(new { temperature = 22, humidity = 88 });
        await Task.Delay(10000);
    }
}