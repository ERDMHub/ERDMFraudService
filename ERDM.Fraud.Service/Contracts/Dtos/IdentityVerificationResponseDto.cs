namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class IdentityVerificationResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string VerificationId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string VerificationStatus { get; set; } = string.Empty;
        public int VerificationScore { get; set; }
        public VerificationDetailsDto VerificationDetails { get; set; } = new();
        public LivenessResultDto? LivenessResult { get; set; }
        public string? BiometricHash { get; set; }
        public DateTime? VerifiedAt { get; set; }
        public string? VerifiedBy { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
