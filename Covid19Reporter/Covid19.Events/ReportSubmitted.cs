using Covid19.Primitives;

namespace Covid19.Events
{
    public sealed class ReportSubmitted
    {
        public ReportSubmitted(
            string email,
            Symptoms symptoms,
            LatLng position)
        {
            Email = email ?? throw new System.ArgumentNullException(nameof(email));
            Symptoms = symptoms;
            Position = position;
        }

        public string Email { get; }
        public Symptoms Symptoms { get; }
        public LatLng Position { get; }
    }
}
