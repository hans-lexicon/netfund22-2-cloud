using System;
using AzureFunctions.Contexts;
using AzureFunctions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions.Functions
{
    public class SaveDataToCosmosDB
    {
        private readonly ILogger _logger;
        private readonly CosmosDbContext _context;

        public SaveDataToCosmosDB(ILoggerFactory loggerFactory, CosmosDbContext context)
        {
            _logger = loggerFactory.CreateLogger<SaveDataToCosmosDB>();
            _context = context;
        }

        [Function("SaveDataToCosmosDB")]
        public async Task Run([EventHubTrigger("iothub-ehub-netfund22-24646018-c5cbb5a1cf", Connection = "EventHubEndpoint", ConsumerGroup = "cosmosdb")] string[] input, FunctionContext context)
        {
            for (int i = 0; i < input.Length; i++)
            {
                var message = input[i];
                _logger.LogInformation($"Message to Save: {message}");

                var systemPropertiesArray = context.BindingContext.BindingData["systemPropertiesArray"]?.ToString();
                var systemProperties = JsonConvert.DeserializeObject<dynamic>(systemPropertiesArray!);
                _logger.LogInformation($"System Properties to Save {systemProperties}");

                _context.Telemetry.Add(new TelemetryEntity
                {
                    Data = message,
                    SystemProperties = systemPropertiesArray,
                });

                await _context.SaveChangesAsync();
            }
        }
    }
}
