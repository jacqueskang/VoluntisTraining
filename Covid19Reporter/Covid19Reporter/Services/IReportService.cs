using Covid19.Events;
using System.Threading.Tasks;

namespace Covid19Reporter.Services
{
    public interface IReportService
    {
        Task SubmitReportAsync(ReportSubmitted @event);
    }
}
