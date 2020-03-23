using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Codiv19Functions
{
    public static class Function1
    {
        [FunctionName("analyze-report")]
        public static void Run([QueueTrigger("queue-new-reports", Connection = "AzureWebJobsStorage")]EventGridEvent @event, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {@event.Data}");
        }
    }
}
