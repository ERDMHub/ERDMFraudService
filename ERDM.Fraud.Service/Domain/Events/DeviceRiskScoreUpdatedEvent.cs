using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class DeviceRiskScoreUpdatedEvent : FraudDomainEventBase
    {
        public DeviceRiskScoreUpdatedEvent(DeviceFingerprint device, int newScore, RiskLevel riskLevel)
        {
            EntityId = device.Id;
            EntityType = nameof(DeviceFingerprint);
            DeviceId = device.DeviceId;
            NewScore = newScore;
            RiskLevel = riskLevel;
            UpdatedAt = DateTime.UtcNow;
        }

        public string DeviceId { get; }
        public int NewScore { get; }
        public RiskLevel RiskLevel { get; }
        public DateTime UpdatedAt { get; }
    }

}
