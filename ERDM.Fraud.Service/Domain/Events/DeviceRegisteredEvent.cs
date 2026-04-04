using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;

namespace ERDM.Fraud.Service.Domain.Events
{
    // Device Events
    public class DeviceRegisteredEvent : FraudDomainEventBase
    {
        public DeviceRegisteredEvent(DeviceFingerprint device)
        {
            EntityId = device.Id;
            EntityType = nameof(DeviceFingerprint);
            DeviceId = device.DeviceId;
            FingerprintHash = device.FingerprintHash;
            DeviceType = device.DeviceType;
            RegisteredAt = device.FirstSeenAt;
        }

        public string DeviceId { get; }
        public string FingerprintHash { get; }
        public DeviceType DeviceType { get; }
        public DateTime RegisteredAt { get; }
    }
   
    public class SanctionHit
    {
        public string ListName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MatchType { get; set; } = string.Empty;
        public decimal MatchScore { get; set; }
        public DateTime MatchedAt { get; set; }
    }
}