using System.Collections.Generic;
using System.Linq;

namespace Codiv19Reporter.Services
{
    public class ReportDto
    {
        private readonly HashSet<string> _symptoms = new HashSet<string>();

        public static ReportDto WithoutSymptoms()
            => new ReportDto(false);

        public static ReportDto WithSymptoms(bool fever, bool cough, bool headache, bool others)
        {
            var report = new ReportDto(true);
            if (fever)
            {
                report._symptoms.Add("Fever");
            }
            if (cough)
            {
                report._symptoms.Add("Cough");
            }
            if (headache)
            {
                report._symptoms.Add("Headache");
            }
            if (others)
            {
                report._symptoms.Add("Others");
            }

            return report;
        }

        private ReportDto(bool haveSymptoms)
        {
            HaveSymptoms = haveSymptoms;
        }

        public bool HaveSymptoms { get; }

        public IEnumerable<string> Symptoms => _symptoms.ToList().AsReadOnly();
    }
}
