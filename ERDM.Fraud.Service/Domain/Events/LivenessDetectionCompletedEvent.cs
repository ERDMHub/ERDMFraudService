using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.ValueObjects;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class LivenessDetectionCompletedEvent : FraudDomainEventBase
    {
        public LivenessDetectionCompletedEvent(IdentityVerification verification, LivenessResult result)
        {
            EntityId = verification.Id;
            EntityType = nameof(IdentityVerification);
            VerificationId = verification.VerificationId;
            IsAlive = result.IsAlive;
            ConfidenceScore = result.ConfidenceScore;
            CompletedAt = DateTime.UtcNow;
        }

        public string VerificationId { get; }
        public bool IsAlive { get; }
        public decimal ConfidenceScore { get; }
        public DateTime CompletedAt { get; }
    }
}
