using ERDM.Fraud.Service.Domain.Entities;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class DeviceAssociatedEvent : FraudDomainEventBase
    {
        public DeviceAssociatedEvent(DeviceFingerprint device, string customerId)
        {
            EntityId = device.Id;
            EntityType = nameof(DeviceFingerprint);
            DeviceId = device.DeviceId;
            CustomerId = customerId;
            AssociatedAt = DateTime.UtcNow;
        }

        public string DeviceId { get; }
        public string CustomerId { get; }
        public DateTime AssociatedAt { get; }
    }
}
