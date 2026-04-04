using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.ValueObjects;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class BehavioralPatternDetectedEvent : FraudDomainEventBase
    {
        public BehavioralPatternDetectedEvent(DeviceFingerprint device, BehavioralPattern pattern)
        {
            EntityId = device.Id;
            EntityType = nameof(DeviceFingerprint);
            DeviceId = device.DeviceId;
            PatternType = pattern.PatternType;
            PatternValue = pattern.PatternValue;
            Confidence = pattern.Confidence;
            DetectedAt = pattern.DetectedAt;
        }

        public string DeviceId { get; }
        public string PatternType { get; }
        public string PatternValue { get; }
        public decimal Confidence { get; }
        public DateTime DetectedAt { get; }
    }
}
