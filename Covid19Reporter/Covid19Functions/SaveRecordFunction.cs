using Covid19.Domain;
using Covid19.Events;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Functions
{
    public static class SaveRecordFunction
    {
        [FunctionName("save-record")]
        public static async Task Run(
            [QueueTrigger("queue-new-records", Connection = "AzureWebJobsStorage")]
            EventGridEvent @event,
            ILogger log)
        {
            ReportAnalyzed report = ((JObject)@event.Data).ToObject<ReportAnalyzed>();

            log.LogInformation("Querying patient {UserName}...", report.UserName);
            var client = new CosmosClient(Environment.GetEnvironmentVariable("CosmosDBConnectionString"));
            Container container = client.GetDatabase("cosmosdb-covid19").GetContainer("patients");
            QueryDefinition query = new QueryDefinition(@"SELECT
    c.id,
    c.records
FROM
    c
WHERE
    c.id = @id")
                .WithParameter("@id", report.UserName);
            FeedResponse<Patient> results = await container.GetItemQueryIterator<Patient>(query).ReadNextAsync();
            Patient patient = results.FirstOrDefault() ?? Patient.Create(report.UserName);
            patient.AddRecord(report.Symptoms, report.Position, report.SubmitTime, report.Recommendation, report.IsSuspected);

            log.LogInformation("Upserting patient {UserName}...", report.UserName);
            await container.UpsertItemAsync(patient);
        }
    }
}
