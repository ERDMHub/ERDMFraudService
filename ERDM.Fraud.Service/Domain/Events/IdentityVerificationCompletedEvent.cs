using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class IdentityVerificationCompletedEvent : FraudDomainEventBase
    {
        public IdentityVerificationCompletedEvent(IdentityVerification verification)
        {
            EntityId = verification.Id;
            EntityType = nameof(IdentityVerification);
            VerificationId = verification.VerificationId;
            CustomerId = verification.CustomerId;
            Status = verification.VerificationStatus;
            Score = verification.VerificationScore;
            CompletedAt = verification.VerifiedAt ?? DateTime.UtcNow;
        }

        public string VerificationId { get; }
        public string CustomerId { get; }
        public VerificationStatus Status { get; }
        public int Score { get; }
        public DateTime CompletedAt { get; }
    }

}
