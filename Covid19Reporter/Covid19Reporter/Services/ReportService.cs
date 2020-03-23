using Covid19.Events;
using Covid19Reporter.Options;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Covid19Reporter.Services
{
    public class ReportService : IReportService
    {
        private readonly IOptionsMonitor<TopicOptions> _options;

        public ReportService(IOptionsMonitor<TopicOptions> options)
        {
            _options = options;
        }

        public async Task SubmitReportAsync(ReportSubmitted @event)
        {
            TopicOptions topic = _options.CurrentValue;
            var credentials = new TopicCredentials(topic.AccessKey);
            var client = new EventGridClient(credentials);

            await client.PublishEventsAsync(topic.Endpoint.Host, new[] {
                new EventGridEvent
                {
                    Id = Guid.NewGuid().ToString(),
                    Subject = "Report submitted",
                    EventTime = DateTime.UtcNow,
                    EventType = nameof(ReportSubmitted),
                    DataVersion = "1.0",
                    Data = JsonConvert.SerializeObject(@event)
                }
            });
        }
    }
}
