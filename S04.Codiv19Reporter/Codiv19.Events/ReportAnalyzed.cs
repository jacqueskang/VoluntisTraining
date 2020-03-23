namespace Codiv19.Events
{
    public class ReportAnalyzed
    {
        public ReportAnalyzed(string email, bool isSuspected, string recommendation)
        {
            Email = email;
            IsSuspected = isSuspected;
            Recommendation = recommendation;
        }

        public string Email { get; }
        public bool IsSuspected { get; }
        public string Recommendation { get; }
    }
}
