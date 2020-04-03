using Covid19.Primitives;
using System;

namespace Covid19.Domain
{
    public class MedicalRecord
    {
        public MedicalRecord(
            Symptoms symptoms,
            LatLng position,
            DateTime submitTime,
            string recommendation,
            bool isSuspected)
        {
            Symptoms = symptoms;
            Position = position;
            SubmitTime = submitTime;
            Recommendation = recommendation;
            IsSuspected = isSuspected;
        }

        public Symptoms Symptoms { get; }
        public LatLng Position { get; }
        public DateTime SubmitTime { get; }
        public string Recommendation { get; }
        public bool IsSuspected { get; }
    }
}