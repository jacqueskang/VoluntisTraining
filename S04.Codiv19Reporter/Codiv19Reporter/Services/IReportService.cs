using System.Threading.Tasks;

namespace Codiv19Reporter.Services
{
    public interface IReportService
    {
        Task SendReportAsync(ReportDto report);
    }
}
