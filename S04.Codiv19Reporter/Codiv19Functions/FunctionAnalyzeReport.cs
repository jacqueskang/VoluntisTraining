using Codiv19.Events;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace Codiv19Functions
{
    public static class FunctionAnalyzeReport
    {
        [FunctionName("analyze-report")]
        public static void Run([QueueTrigger("queue-new-reports", Connection = "AzureWebJobsStorage")]EventGridEvent envelope, ILogger log)
        {
            string data = envelope.Data as string;
            if (string.IsNullOrEmpty(data))
            {
                throw new InvalidOperationException("Data is empty");
            }

            log.LogInformation($"Received report {data}. Analyzing...");

            ReportSubmitted reportSubmitted = JsonConvert.DeserializeObject<ReportSubmitted>(data);
            ReportAnalyzed reportAnalyzed;
            if (reportSubmitted.HaveSymptoms)
            {
                if (reportSubmitted.Symptoms.Count() > 1)
                {
                    reportAnalyzed = new ReportAnalyzed(reportSubmitted.Email, true, "Restez chez-vous et appeler votre médecin.");
                }
                else
                {
                    reportAnalyzed = new ReportAnalyzed(reportSubmitted.Email, false, "Surveillez-vous.");
                }
            }
            else
            {
                reportAnalyzed = new ReportAnalyzed(reportSubmitted.Email, false, "Vous êtes très bien.");
            }

            log.LogInformation($"TODO: submit report {reportAnalyzed}");
        }
    }
}
