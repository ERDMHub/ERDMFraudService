using ERDM.Fraud.Service.Domain.Entities;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class BiometricRegisteredEvent : FraudDomainEventBase
    {
        public BiometricRegisteredEvent(IdentityVerification verification)
        {
            EntityId = verification.Id;
            EntityType = nameof(IdentityVerification);
            VerificationId = verification.VerificationId;
            CustomerId = verification.CustomerId;
            RegisteredAt = DateTime.UtcNow;
        }

        public string VerificationId { get; }
        public string CustomerId { get; }
        public DateTime RegisteredAt { get; }
    }
}
