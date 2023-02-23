using System.Net;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

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
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "devices")] HttpRequestData req)
        {
            var deviceId = await new StreamReader(req.Body).ReadToEndAsync();

            if (!string.IsNullOrEmpty(deviceId))
            {
                var device = await _registryManager.GetDeviceAsync(deviceId);
                device ??= await _registryManager.AddDeviceAsync(new Device(deviceId));

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync($"{Environment.GetEnvironmentVariable("IotHub")?.Split(";")[0]};DeviceId={device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}");

                return response;
            }

            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
