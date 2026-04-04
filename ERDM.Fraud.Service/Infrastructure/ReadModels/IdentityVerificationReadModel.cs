namespace ERDM.Fraud.Service.Infrastructure.ReadModels
{
    public class IdentityVerificationReadModel
    {
        public string Id { get; set; } = string.Empty;
        public string VerificationId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string VerificationStatus { get; set; } = string.Empty;
        public int VerificationScore { get; set; }

        // Document Details
        public string DocumentImageUrl { get; set; } = string.Empty;
        public string ExtractedFullName { get; set; } = string.Empty;
        public DateTime? ExtractedDateOfBirth { get; set; }
        public DateTime? DocumentIssueDate { get; set; }
        public DateTime? DocumentExpiryDate { get; set; }
        public string ExtractedAddress { get; set; } = string.Empty;

        // Verification Details
        public bool DocumentAuthentic { get; set; }
        public bool FaceMatch { get; set; }
        public decimal FaceMatchScore { get; set; }
        public bool DataConsistent { get; set; }
        public List<string> Flags { get; set; } = new();
        public List<string> Warnings { get; set; } = new();

        // Liveness Detection
        public bool? LivenessPassed { get; set; }
        public decimal? LivenessConfidenceScore { get; set; }
        public string? LivenessMethod { get; set; }
        public List<string> LivenessChecksPassed { get; set; } = new();
        public List<string> LivenessChecksFailed { get; set; } = new();

        // Biometric
        public string? BiometricHash { get; set; }
        public bool BiometricRegistered { get; set; }

        // Dates
        public DateTime InitiatedAt { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? VerifiedBy { get; set; }
        public DateTime ExpiresAt { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Metadata
        public Dictionary<string, object> Metadata { get; set; } = new();
        public List<string> Tags { get; set; } = new();
    }
}
