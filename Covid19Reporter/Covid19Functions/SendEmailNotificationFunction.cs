using Covid19.Events;
using Covid19.Primitives;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
using System;
using System.Web;

namespace Covid19Functions
{
    public static class SendEmailNotificationFunction
    {
        [FunctionName("send-email-notification")]
        [return: SendGrid(ApiKey = "SendGridKey")]
        public static SendGridMessage Run(
            [QueueTrigger("queue-email-notifications", Connection = "AzureWebJobsStorage")]
            EventGridEvent @event,
            ILogger log)
        {
            log.LogInformation("Deserializing report...");
            ReportAnalyzed analysis = JsonConvert.DeserializeObject<ReportAnalyzed>(@event.Data as string);

            log.LogInformation("Sending email notification...");

            string plainTextContent = "Vous avez saisi les symptômes suivant(s) :\n";
            string htmlContent = "<p>Vous avez saisi les symptômes suivant(s) : <ul>";
            foreach (Enum value in Enum.GetValues(typeof(Symptoms)))
            {
                if (analysis.Symptoms.HasFlag(value))
                {
                    plainTextContent += $" - {value.ToString()}\n";
                    htmlContent += $"<li>{value.ToString()}</li>";
                }
            }
            plainTextContent += $"\nNotre recommendation est : {analysis.Recommendation}";
            htmlContent += $"</ul></p><p>Notre recommendation est : <strong>{analysis.Recommendation}</strong></p>";

            var from = new EmailAddress("noreply@codiv-19.com", "Covid-19 Analyzer");
            var to = new EmailAddress(analysis.Email);
            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, "Notre recommendation", plainTextContent, htmlContent);
            return msg;
        }
    }
}
