using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Functions
{
    public class ViewTelemetryData
    {
        private readonly ILogger _logger;

        public ViewTelemetryData(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ViewTelemetryData>();
        }

        [Function("ViewTelemetryData")]
        public void Run([EventHubTrigger("iothub-ehub-netfund22-24646018-c5cbb5a1cf", Connection = "EventHubEndpoint", ConsumerGroup = "view")] string[] input)
        {
            _logger.LogInformation($"Message: {input[0]}");
        }
    }
}
