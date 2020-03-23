using Codiv19.Events;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System.Web;

namespace Codiv19Functions
{
    public static class SendEmailNotificationFunction
    {
        [FunctionName("send-email-notification")]
        public static async Task Run([QueueTrigger("queue-email-notifications", Connection = "AzureWebJobsStorage")]EventGridEvent @event, ILogger log)
        {
            log.LogInformation("Deserializing report...");
            var analysis = JsonConvert.DeserializeObject<ReportAnalyzed>(@event.Data as string);

            log.LogInformation("Sending email notification...");

            var client = new SendGridClient("<ask-jia-for-it>");
            var from = new EmailAddress("noreply@codiv-19.com", "Codiv-19 Analyzer");
            var to = new EmailAddress(analysis.Email);
            var msg = MailHelper.CreateSingleEmail(from, to, "Vos résultats", analysis.Recommendation,
                $"<h2>Vos résultats:</h2>" +
                $"<strong>{HttpUtility.HtmlEncode(analysis.Recommendation)}</strong>");
            var response = await client.SendEmailAsync(msg);

            log.LogInformation($"Email sent. Result: {response.StatusCode}");
        }
    }
}
