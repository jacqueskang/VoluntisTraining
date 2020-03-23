using System.Collections.Generic;

namespace Codiv19Reporter.Services
{
    public class ReportDto
    {
        public static ReportDto WithoutSymptoms(string email)
            => new ReportDto
            {
                Email = email,
                HaveSymptoms = false
            };

        public static ReportDto WithSymptoms(string email, bool fever, bool cough, bool headache, bool others)
        {
            var report = new ReportDto
            {
                Email = email,
                HaveSymptoms = true
            };
            if (fever)
            {
                report.Symptoms.Add("Fever");
            }
            if (cough)
            {
                report.Symptoms.Add("Cough");
            }
            if (headache)
            {
                report.Symptoms.Add("Headache");
            }
            if (others)
            {
                report.Symptoms.Add("Others");
            }

            return report;
        }

        public string Email { get; set; }
        public bool HaveSymptoms { get; set; }
        public List<string> Symptoms { get; set; } = new List<string>();
    }
}
