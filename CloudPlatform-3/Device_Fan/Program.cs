using Device_Shared.Services;

var deviceService = new DeviceService();
await Task.Delay(2000);

var device = deviceService.CreateDevice("Dson Bladeless Fan", "fan");
if(device != null)
{
    Console.WriteLine($"device {device.DeviceId} was created");

    if (device!.ConnectionString == null)
    {
        device.ConnectionString = await deviceService.RegisterDeviceAsync(device.DeviceId, "http://localhost:7072/api/devices");
        Console.WriteLine($"connectionsstring generated {device.ConnectionString}");
    }


    if (!string.IsNullOrEmpty(device.ConnectionString))
    {
        await deviceService.InitializeAsync(device);
        Console.WriteLine($"device initialized.");


        
    }

    Console.WriteLine($"device running.");

}
Console.WriteLine($"method handlers initialized.");
await deviceService.InitializeMethodHandlersAsync();
Console.ReadKey();

