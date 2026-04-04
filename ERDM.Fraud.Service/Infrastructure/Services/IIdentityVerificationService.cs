using ERDM.Fraud.Service.Contracts.Dtos;

namespace ERDM.Fraud.Service.Infrastructure.Services
{
    public interface IIdentityVerificationService
    {
        Task<VerificationResult> VerifyDocument(string documentImageUrl, string selfieImageUrl, string documentType);
        Task<DocumentValidationResult> ValidateDocument(string imageUrl, string documentType);
        Task<FaceMatchResult> MatchFaceToDocument(string selfieUrl, string documentImageUrl);
        Task<DataExtractionResult> ExtractDocumentData(string imageUrl);
    }

    public class VerificationResult
    {
        public int Score { get; set; }
        public VerificationDetailsDto Details { get; set; } = new();
        public bool IsVerified => Score >= 70;
    }

    public class DocumentValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> ValidationFlags { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
        public decimal AuthenticityScore { get; set; }
    }

    public class FaceMatchResult
    {
        public bool IsMatch { get; set; }
        public decimal MatchScore { get; set; }
        public Dictionary<string, decimal> FacialFeatures { get; set; } = new();
    }

    public class DataExtractionResult
    {
        public string FullName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public DateTime? DateOfBirth { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public Dictionary<string, string> ExtractedFields { get; set; } = new();
    }

}
