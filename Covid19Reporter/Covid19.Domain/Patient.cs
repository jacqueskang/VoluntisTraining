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
            string id,
            IEnumerable<MedicalRecord> records)
        {
            if (records is null)
            {
                throw new ArgumentNullException(nameof(records));
            }

            Id = id ?? throw new ArgumentNullException(nameof(id));
            _records = records.ToList();
        }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; }

        [JsonProperty(PropertyName = "records")]
        public IEnumerable<MedicalRecord> Records => _records.AsReadOnly();

        [JsonProperty(PropertyName = "position")]
        public LatLng Position => LatestRecord?.Position;

        [JsonProperty(PropertyName = "isSuspected")]
        public bool? IsSuspected => LatestRecord?.IsSuspected;

        [JsonProperty(PropertyName = "updateTime")]
        public DateTime? UpdateTime => LatestRecord?.SubmitTime;

        public static Patient Create(string userName)
            => new Patient(userName, Array.Empty<MedicalRecord>());

        public void AddRecord(Symptoms symptoms, LatLng position, DateTime submitTime, string recommendation,
            bool isSuspected)
        {
            _records.Add(new MedicalRecord(symptoms, position, submitTime, recommendation, isSuspected));
        }

        private MedicalRecord LatestRecord => _records.OrderByDescending(x => x.SubmitTime).FirstOrDefault();
    }
}
