using Covid19.Primitives;
using System;

namespace Covid19.Events
{
    public class ReportAnalyzed
    {
        public ReportAnalyzed(string userName, Symptoms symptoms, string email, LatLng position, DateTime submitTime,
            string recommendation, bool isSuspected)
        {
            UserName = userName;
            Symptoms = symptoms;
            Email = email;
            Position = position;
            SubmitTime = submitTime;
            IsSuspected = isSuspected;
            Recommendation = recommendation;
        }

        public string UserName { get; }
        public Symptoms Symptoms { get; }
        public string Email { get; }
        public LatLng Position { get; }
        public DateTime SubmitTime { get; }
        public bool IsSuspected { get; }
        public string Recommendation { get; }
    }
}
