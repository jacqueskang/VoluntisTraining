using Covid19.Primitives;

namespace Covid19.Events
{
    public sealed class ReportSubmitted
    {
        public ReportSubmitted(
            string email,
            Symptoms symptoms)
        {
            Email = email ?? throw new System.ArgumentNullException(nameof(email));
            Symptoms = symptoms;
        }

        public string Email { get; }
        public Symptoms Symptoms { get; }
    }
}
