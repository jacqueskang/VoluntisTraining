using Covid19.Events;
using Covid19.Primitives;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace Covid19Functions
{
    public static class AnalyzeReportFunction
    {
        [FunctionName("analyze-report")]
        [return: EventGrid(TopicEndpointUri = "Topic:Endpoint", TopicKeySetting = "Topic:AccessKey")]
        public static EventGridEvent Run(
            [QueueTrigger("queue-new-reports", Connection = "AzureWebJobsStorage")]
            EventGridEvent @event,
            ILogger log)
        {
            log.LogInformation($"Received report. Analyzing...");

            ReportSubmitted report = ((JObject)@event.Data).ToObject<ReportSubmitted>();
            ReportAnalyzed analysis;
            if (report.Symptoms.HasFlag(Symptoms.BreathingDifficulty))
            {
                analysis = new ReportAnalyzed(report.UserName, report.Symptoms, report.Email, report.Position, @event.EventTime,
                    "Appelez 15", isSuspected: true);
            }
            else if (Enum.GetValues(typeof(Symptoms)).Cast<Symptoms>().Count(x => report.Symptoms.HasFlag(x)) > 1)
            {
                analysis = new ReportAnalyzed(report.UserName, report.Symptoms, report.Email, report.Position, @event.EventTime,
                    "Appeler votre médecin.", isSuspected: true);
            }
            else if (report.Symptoms != Symptoms.None)
            {
                analysis = new ReportAnalyzed(report.UserName, report.Symptoms, report.Email, report.Position, @event.EventTime,
                    "Continuez à surveiller votre état et restez chez-vous.", isSuspected: false);
            }
            else
            {
                analysis = new ReportAnalyzed(report.UserName, report.Symptoms, report.Email, report.Position, @event.EventTime,
                    "Vous êtes très bien ne vous inquiétez pas.", isSuspected: false);
            }

            log.LogInformation($"Analyzed. Publishing results...");
            return new EventGridEvent
            {
                Id = Guid.NewGuid().ToString(),
                Subject = "Report analyzed",
                EventTime = DateTime.UtcNow,
                EventType = nameof(ReportAnalyzed),
                DataVersion = "1.0",
                Data = JObject.FromObject(analysis)
            };
        }
    }
}
