using Covid19.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Covid19.Domain
{
    public class Patient
    {
        private readonly List<MedicalRecord> _records;

        public Patient(
            Guid id,
            string userName,
            IEnumerable<MedicalRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            Id = id;
            UserName = userName ?? throw new ArgumentNullException(nameof(id));
            _records = records.ToList();
        }

        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; }

        [JsonProperty(PropertyName = "records")]
        public IEnumerable<MedicalRecord> Records => _records.AsReadOnly();

        [JsonProperty(PropertyName = "position")]
        public LatLng Position => LatestRecord?.Position;

        [JsonProperty(PropertyName = "isSuspected")]
        public bool? IsSuspected => LatestRecord?.IsSuspected;

        [JsonProperty(PropertyName = "updateTime")]
        public DateTime? UpdateTime => LatestRecord?.SubmitTime;

        public static Patient Create(string userName)
            => new Patient(Guid.NewGuid(), userName, Array.Empty<MedicalRecord>());

        public void AddRecord(Symptoms symptoms, LatLng position, DateTime submitTime, string recommendation,
            bool isSuspected)
        {
            _records.Add(new MedicalRecord(symptoms, position, submitTime, recommendation, isSuspected));
        }

        private MedicalRecord LatestRecord => _records.OrderByDescending(x => x.SubmitTime).FirstOrDefault();
    }
}
