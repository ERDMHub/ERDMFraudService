using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;

namespace ERDM.Fraud.Service.Domain.Events
{
    // Identity Verification Events
    public class IdentityVerificationInitiatedEvent : FraudDomainEventBase
    {
        public IdentityVerificationInitiatedEvent(IdentityVerification verification)
        {
            EntityId = verification.Id;
            EntityType = nameof(IdentityVerification);
            VerificationId = verification.VerificationId;
            CustomerId = verification.CustomerId;
            DocumentType = verification.DocumentType;
            InitiatedAt = DateTime.UtcNow;
        }

        public string VerificationId { get; }
        public string CustomerId { get; }
        public DocumentType DocumentType { get; }
        public DateTime InitiatedAt { get; }
    }

}
