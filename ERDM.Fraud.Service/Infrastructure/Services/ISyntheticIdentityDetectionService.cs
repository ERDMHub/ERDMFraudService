using ERDM.Fraud.Service.Domain.Entities;

namespace ERDM.Fraud.Service.Infrastructure.Services
{
    public interface ISyntheticIdentityDetectionService
    {
        Task<SyntheticIdentityDetectionResult> DetectSyntheticIdentity(CustomerProfileData profileData);
        Task<List<SyntheticIdentityIndicator>> GetIndicators();
        Task<IdentityValidationResult> ValidateIdentityAttributes(CustomerProfileData profileData);
    }

    public class SyntheticIdentityDetectionResult
    {
        public bool IsSynthetic { get; set; }
        public decimal ConfidenceScore { get; set; }
        public List<string> Indicators { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public Dictionary<string, decimal> FactorScores { get; set; } = new();
    }

    public class SyntheticIdentityIndicator
    {
        public string IndicatorId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Weight { get; set; }
    }

    public class IdentityValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> ValidationErrors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
        public Dictionary<string, bool> AttributeChecks { get; set; } = new();
    }
}
