using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Functions
{
    public class ViewIotHubMessages
    {
        private readonly ILogger _logger;

        public ViewIotHubMessages(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<ViewIotHubMessages>();
        }

        [Function("ViewIotHubMessages")]
        public void Run([EventHubTrigger("iothub-ehub-netfund22-24604377-6d7bd290ab", Connection = "IotHubEndpoint", ConsumerGroup = "viewmessages")] string[] input)
        {
            _logger.LogInformation($"Message: {input[0]}");
        }
    }
}
