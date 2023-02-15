using System.Net;
using AzureFunctions.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions.Functions
{
    public class RegisterDevice
    {
        private RegistryManager _registryManager;

        public RegisterDevice()
        {
            _registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));
        }

        [Function("RegisterDevice")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var deviceSettings = JsonConvert.DeserializeObject<DeviceSettings>(await new StreamReader(req.Body).ReadToEndAsync());
        
            if (!string.IsNullOrEmpty(deviceSettings?.DeviceId))
            {
                var device = await _registryManager.GetDeviceAsync(deviceSettings.DeviceId);
                device ??= await _registryManager.AddDeviceAsync(new Device(deviceSettings.DeviceId));

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync($"{Environment.GetEnvironmentVariable("IotHub")?.Split(";")[0]};DeviceId={device!.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}");
                return response;
            }

            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
