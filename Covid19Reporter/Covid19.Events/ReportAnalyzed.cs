using Covid19.Primitives;

namespace Covid19.Events
{
    public class ReportAnalyzed
    {
        public ReportAnalyzed(string userName, string email, LatLng position, bool isSuspected, string recommendation)
        {
            UserName = userName;
            Email = email;
            Position = position;
            IsSuspected = isSuspected;
            Recommendation = recommendation;
        }

        public string UserName { get; }
        public string Email { get; }
        public LatLng Position { get; }
        public bool IsSuspected { get; }
        public string Recommendation { get; }
    }
}
