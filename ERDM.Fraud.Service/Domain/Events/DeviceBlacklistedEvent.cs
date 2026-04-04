using ERDM.Fraud.Service.Domain.Entities;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class DeviceBlacklistedEvent : FraudDomainEventBase
    {
        public DeviceBlacklistedEvent(DeviceFingerprint device, string reason)
        {
            EntityId = device.Id;
            EntityType = nameof(DeviceFingerprint);
            DeviceId = device.DeviceId;
            Reason = reason;
            BlacklistedAt = DateTime.UtcNow;
        }

        public string DeviceId { get; }
        public string Reason { get; }
        public DateTime BlacklistedAt { get; }
    }
}
