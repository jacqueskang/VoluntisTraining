namespace Codiv19.Events
{
    public class ReportAnalyzed
    {
        public ReportAnalyzed(string email, bool isInfected, string recommendation)
        {
            Email = email;
            IsInfected = isInfected;
            Recommendation = recommendation;
        }

        public string Email { get; }
        public bool IsInfected { get; }
        public string Recommendation { get; }
    }
}
