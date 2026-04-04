using ERDM.Fraud.Service.Domain.Common;
using ERDM.Fraud.Service.Domain.Enums;
using ERDM.Fraud.Service.Domain.Events;
using ERDM.Fraud.Service.Domain.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Fraud.Service.Domain.Entities
{
    public class IdentityVerification : AggregateRoot
    {
        [BsonElement("verificationId")]
        public string VerificationId { get; private set; }

        [BsonElement("customerId")]
        public string CustomerId { get; private set; }

        [BsonElement("documentType")]
        public DocumentType DocumentType { get; private set; }

        [BsonElement("documentNumber")]
        public string DocumentNumber { get; private set; }

        [BsonElement("verificationStatus")]
        public VerificationStatus VerificationStatus { get; private set; }

        [BsonElement("verificationScore")]
        public int VerificationScore { get; private set; }

        [BsonElement("verificationDetails")]
        public VerificationDetails VerificationDetails { get; private set; }

        [BsonElement("livenessResult")]
        public LivenessResult? LivenessResult { get; private set; }

        [BsonElement("biometricHash")]
        public string? BiometricHash { get; private set; }

        [BsonElement("verifiedAt")]
        public DateTime? VerifiedAt { get; private set; }

        [BsonElement("verifiedBy")]
        public string? VerifiedBy { get; private set; }

        [BsonElement("expiresAt")]
        public DateTime ExpiresAt { get; private set; }

        // Public parameterless constructor for MongoDB deserialization
        public IdentityVerification()
        {
            VerificationDetails = new VerificationDetails();
        }

        public static IdentityVerification Create(
            string customerId,
            DocumentType documentType,
            string documentNumber)
        {
            var verification = new IdentityVerification
            {
                VerificationId = GenerateVerificationId(),
                CustomerId = customerId,
                DocumentType = documentType,
                DocumentNumber = documentNumber,
                VerificationStatus = VerificationStatus.Pending,
                VerificationScore = 0,
                VerificationDetails = new VerificationDetails(),
                ExpiresAt = DateTime.UtcNow.AddDays(30)
            };

            verification.AddDomainEvent(new IdentityVerificationInitiatedEvent(verification));
            return verification;
        }

        public void CompleteVerification(int score, VerificationDetails details, string verifiedBy)
        {
            VerificationScore = score;
            VerificationDetails = details;
            VerificationStatus = score >= 70 ? VerificationStatus.Approved : VerificationStatus.Rejected;
            VerifiedAt = DateTime.UtcNow;
            VerifiedBy = verifiedBy;

            AddDomainEvent(new IdentityVerificationCompletedEvent(this));
        }

        public void AddLivenessResult(LivenessResult result)
        {
            LivenessResult = result;
            AddDomainEvent(new LivenessDetectionCompletedEvent(this, result));
        }

        public void SetBiometricHash(string hash)
        {
            BiometricHash = hash;
            AddDomainEvent(new BiometricRegisteredEvent(this));
        }

        private static string GenerateVerificationId()
        {
            return $"VID-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid():N}";
        }
    }
}