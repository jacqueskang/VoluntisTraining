using Covid19.Events;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Covid19Functions
{
    public static class SavePositionFunction
    {
        [FunctionName("save-positions")]
        [return: CosmosDB(databaseName: "cosmosdb-covid19", collectionName: "positions",
                ConnectionStringSetting = "CosmosDBConnectionString")]
        public static dynamic Run(
            [QueueTrigger("queue-new-positions", Connection = "AzureWebJobsStorage")]
            EventGridEvent @event,
            ILogger log)
        {
            log.LogInformation("Deserializing report...");
            ReportAnalyzed analysis = JsonConvert.DeserializeObject<ReportAnalyzed>(@event.Data as string);

            log.LogInformation("Saving position...");
            return new
            {
                userName = analysis.UserName,
                position = new
                {
                    type = "Point",
                    coordinates = new[]
                    {
                        analysis.Position?.Longitude,
                        analysis.Position?.Latitude
                    }
                },
                isSuspected = analysis.IsSuspected,
                dateTime = @event.EventTime,
            };
        }
    }
}
