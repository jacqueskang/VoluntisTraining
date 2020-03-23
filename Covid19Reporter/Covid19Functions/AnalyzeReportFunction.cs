using Covid19.Events;
using Covid19.Primitives;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Covid19Functions
{
    public static class AnalyzeReportFunction
    {
        [FunctionName("analyze-report")]
        public static async Task Run([QueueTrigger("queue-new-reports", Connection = "AzureWebJobsStorage")]EventGridEvent @event, ILogger log)
        {
            string data = @event.Data as string;
            if (string.IsNullOrEmpty(data))
            {
                throw new InvalidOperationException("Data is empty");
            }

            log.LogInformation($"Received report {data}. Analyzing...");

            ReportSubmitted report = JsonConvert.DeserializeObject<ReportSubmitted>(data);
            ReportAnalyzed analysis;
            if (report.Symptoms.HasFlag(Symptoms.BreathingDifficulty))
            {
                analysis = new ReportAnalyzed(report.Email, true, "Appelez 15");
            }
            else if (Enum.GetValues(typeof(Symptoms)).Cast<Symptoms>().Count(x => report.Symptoms.HasFlag(x)) > 1)
            {
                analysis = new ReportAnalyzed(report.Email, true, "Appeler votre médecin.");
            }
            else if (report.Symptoms != Symptoms.None)
            {
                analysis = new ReportAnalyzed(report.Email, false, "Continuez à surveiller votre état et restez chez-vous.");
            }
            else
            {
                analysis = new ReportAnalyzed(report.Email, false, "Vous êtes très bien ne vous inquiétez pas.");
            }

            log.LogInformation($"Analyzed. Publishing results...");

            string accessKey = Environment.GetEnvironmentVariable("Topic:AccessKey", EnvironmentVariableTarget.Process);
            Uri endpoint = new Uri(Environment.GetEnvironmentVariable("Topic:Endpoint", EnvironmentVariableTarget.Process));
            var credentials = new TopicCredentials(accessKey);
            var client = new EventGridClient(credentials);
            await client.PublishEventsAsync(endpoint.Host, new[] {
                new EventGridEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    Subject = "Report analyzed",
                    EventTime = DateTime.UtcNow,
                    EventType = nameof(ReportAnalyzed),
                    DataVersion = "1.0",
                    Data = JsonConvert.SerializeObject(analysis)
                }
            });
        }
    }
}
