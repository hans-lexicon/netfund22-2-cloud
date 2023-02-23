using System.Net;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Functions
{
    public class GetDevices
    {
        private readonly ILogger _logger;
        private RegistryManager _registryManager;


        public GetDevices(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetDevices>();
            _registryManager = RegistryManager.CreateFromConnectionString(Environment.GetEnvironmentVariable("IotHub"));
        }

        [Function("GetDevices")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "devices")] HttpRequestData req)
        {
            var devices = new List<Twin>();

            var query = _registryManager.CreateQuery("select * from devices");
            while (query.HasMoreResults)
            {
                foreach (var device in await query.GetNextAsTwinAsync())
                {
                    devices.Add(device);
                }
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(devices);

            return response;
        }
    }
}
