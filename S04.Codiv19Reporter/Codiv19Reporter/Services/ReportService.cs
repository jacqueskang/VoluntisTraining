using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Codiv19Reporter.Services
{
    public class ReportService : IReportService
    {
        public Task SendReportAsync(ReportDto report)
        {
            return Task.CompletedTask;
        }
    }
}
