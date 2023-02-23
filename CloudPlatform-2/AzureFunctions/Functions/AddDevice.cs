using System.Net;
using AzureFunctions.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions.Functions
{
    public class AddDevice
    {
        private readonly ILogger _logger;
        private RegistryManager _registryManager;

        public AddDevice(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<AddDevice>();
            _registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));
        }

        [Function("AddDevice")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "devices")] HttpRequestData req)
        {
            var deviceId = await new StreamReader(req.Body).ReadToEndAsync();

            if (!string.IsNullOrEmpty(deviceId))
            {
                var device = await _registryManager.GetDeviceAsync(deviceId);
                device ??= await _registryManager.AddDeviceAsync(new Device(deviceId));

                var response = req.CreateResponse(HttpStatusCode.OK);
                await response.WriteStringAsync($"{Environment.GetEnvironmentVariable("IotHub")?.Split(";")[0]};DeviceId={device.Id};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}");

                _logger.LogInformation($"Device {device.Id} is registered and connectionstring sent to device.");
                return response;
            }

            _logger.LogInformation($"No deviceId was supplied.");
            return req.CreateResponse(HttpStatusCode.BadRequest);
        }
    }
}
