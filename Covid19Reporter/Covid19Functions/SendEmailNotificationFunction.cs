using Covid19.Events;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid.Helpers.Mail;
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

            var from = new EmailAddress("noreply@codiv-19.com", "Covid-19 Analyzer");
            var to = new EmailAddress(analysis.Email);
            SendGridMessage msg = MailHelper.CreateSingleEmail(from, to, "Notre recommendation", analysis.Recommendation,
                $"<h2>Notre recommendation :</h2>" +
                $"<strong>{HttpUtility.HtmlEncode(analysis.Recommendation)}</strong>");
            return msg;
        }
    }
}
