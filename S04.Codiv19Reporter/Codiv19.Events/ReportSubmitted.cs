using System.Collections.Generic;
using System.Linq;

namespace Codiv19.Events
{
    public sealed class ReportSubmitted
    {
        public ReportSubmitted(
            string email,
            bool haveSymptoms,
            IEnumerable<string> symptoms)
        {
            if (symptoms is null)
            {
                throw new System.ArgumentNullException(nameof(symptoms));
            }

            Email = email ?? throw new System.ArgumentNullException(nameof(email));
            HaveSymptoms = haveSymptoms;
            Symptoms = symptoms.ToList().AsReadOnly();
        }

        public string Email { get; }
        public bool HaveSymptoms { get; }
        public IEnumerable<string> Symptoms { get; }

        public static ReportSubmitted WithoutSymptoms(string email)
           => new ReportSubmitted(email, false, new string[0]);

        public static ReportSubmitted WithSymptoms(string email, bool fever, bool cough, bool headache, bool others)
        {
            var symptoms = new List<string>();
            if (fever)
            {
                symptoms.Add("Fever");
            }
            if (cough)
            {
                symptoms.Add("Cough");
            }
            if (headache)
            {
                symptoms.Add("Headache");
            }
            if (others)
            {
                symptoms.Add("Others");
            }
            return new ReportSubmitted(email, true, symptoms);
        }
    }
}
