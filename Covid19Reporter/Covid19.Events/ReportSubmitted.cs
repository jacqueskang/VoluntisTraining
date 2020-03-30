using Covid19.Primitives;

namespace Covid19.Events
{
    public sealed class ReportSubmitted
    {
        public ReportSubmitted(
            string userName,
            string email,
            Symptoms symptoms,
            LatLng position)
        {
            UserName = userName;
            Email = email ?? throw new System.ArgumentNullException(nameof(email));
            Symptoms = symptoms;
            Position = position;
        }

        public string UserName { get; }
        public string Email { get; }
        public Symptoms Symptoms { get; }
        public LatLng Position { get; }
    }
}
