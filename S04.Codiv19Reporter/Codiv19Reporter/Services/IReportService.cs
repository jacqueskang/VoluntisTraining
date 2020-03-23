using Codiv19.Events;
using System.Threading.Tasks;

namespace Codiv19Reporter.Services
{
    public interface IReportService
    {
        Task SubmitReportAsync(ReportSubmitted @event);
    }
}
