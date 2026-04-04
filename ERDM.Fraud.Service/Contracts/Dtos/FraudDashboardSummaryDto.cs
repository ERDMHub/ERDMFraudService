
namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class FraudDashboardSummaryDto
    {
        public int TotalFraudCases { get; set; }
        public int OpenCases { get; set; }
        public int UnderReview { get; set; }
        public int ClosedCases { get; set; }
        public int EscalatedCases { get; set; }

        public int HighRiskDevices { get; set; }
        public int BlacklistedDevices { get; set; }
        public int TotalDevices { get; set; }

        public int PendingVerifications { get; set; }
        public int ApprovedVerifications { get; set; }
        public int RejectedVerifications { get; set; }

        public int SyntheticIdentitiesDetected { get; set; }
        public int SanctionsHits { get; set; }

        public decimal AverageRiskScore { get; set; }
        public decimal TotalPotentialLoss { get; set; }

        public List<FraudTypeSummaryDto> FraudTypeBreakdown { get; set; } = new();
        public List<DailyFraudSummaryDto> Last7Days { get; set; } = new();
        public DateTime AsOfDate { get; set; }
    }

    public class FraudTypeSummaryDto
    {
        public string FraudType { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    public class DailyFraudSummaryDto
    {
        public DateTime Date { get; set; }
        public int CasesCount { get; set; }
        public int HighRiskCount { get; set; }
    }

    public class FraudTrendsDto
    {
        public List<MonthlyFraudTrendDto> MonthlyTrends { get; set; } = new();
        public List<FraudTypeTrendDto> FraudTypeTrends { get; set; } = new();
        public List<RiskScoreTrendDto> RiskScoreTrends { get; set; } = new();
        public PredictionDto? NextMonthPrediction { get; set; }
    }

    public class MonthlyFraudTrendDto
    {
        public string Month { get; set; } = string.Empty;
        public int Year { get; set; }
        public int CasesCount { get; set; }
        public decimal AverageRiskScore { get; set; }
        public int UniqueCustomers { get; set; }
    }

    public class FraudTypeTrendDto
    {
        public string FraudType { get; set; } = string.Empty;
        public List<int> MonthlyCounts { get; set; } = new();
        public decimal GrowthRate { get; set; }
    }

    public class RiskScoreTrendDto
    {
        public DateTime Date { get; set; }
        public decimal AverageRiskScore { get; set; }
        public int TotalCases { get; set; }
    }

    public class PredictionDto
    {
        public int PredictedCases { get; set; }
        public decimal PredictedRiskScore { get; set; }
        public string ConfidenceLevel { get; set; } = string.Empty;
    }

    public class TopFraudTypeDto
    {
        public string FraudType { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
        public decimal AverageRiskScore { get; set; }
        public decimal MonthOverMonthChange { get; set; }
    }

    public class RiskDistributionDto
    {
        public int VeryLowRiskCount { get; set; }
        public int LowRiskCount { get; set; }
        public int MediumRiskCount { get; set; }
        public int HighRiskCount { get; set; }
        public int VeryHighRiskCount { get; set; }
        public Dictionary<string, int> RiskByFraudType { get; set; } = new();
        public Dictionary<string, int> RiskByDeviceType { get; set; } = new();
    }

    public class DeviceStatisticsDto
    {
        public int TotalDevices { get; set; }
        public int UniqueDevices { get; set; }
        public int BlacklistedDevices { get; set; }
        public int HighRiskDevices { get; set; }
        public Dictionary<string, int> DevicesByType { get; set; } = new();
        public Dictionary<string, int> DevicesByOS { get; set; } = new();
        public Dictionary<string, int> DevicesByCountry { get; set; } = new();
        public int DevicesWithVPN { get; set; }
        public int DevicesWithProxy { get; set; }
        public int EmulatorDevices { get; set; }
        public int RootedDevices { get; set; }
        public decimal AverageRiskScore { get; set; }
        public List<DeviceRiskDistributionDto> RiskDistribution { get; set; } = new();
    }

    public class DeviceRiskDistributionDto
    {
        public string RiskLevel { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    public class VerificationStatisticsDto
    {
        public int TotalVerifications { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
        public int Pending { get; set; }
        public int Expired { get; set; }
        public decimal AverageScore { get; set; }
        public Dictionary<string, int> VerificationsByDocumentType { get; set; } = new();
        public Dictionary<string, int> VerificationsByStatus { get; set; } = new();
        public List<DailyVerificationSummaryDto> DailyTrend { get; set; } = new();
        public decimal LivenessPassRate { get; set; }
        public int BiometricRegistrations { get; set; }
    }

    public class DailyVerificationSummaryDto
    {
        public DateTime Date { get; set; }
        public int Total { get; set; }
        public int Approved { get; set; }
        public int Rejected { get; set; }
    }

    public class SanctionsListDto
    {
        public string ListId { get; set; } = string.Empty;
        public string ListName { get; set; } = string.Empty;
        public string ListType { get; set; } = string.Empty;
        public int TotalEntries { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class SyntheticIdentityIndicatorDto
    {
        public string IndicatorId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Weight { get; set; }
    }

    public class CustomerSyntheticRiskDto
    {
        public string CustomerId { get; set; } = string.Empty;
        public bool IsSynthetic { get; set; }
        public decimal ConfidenceScore { get; set; }
        public List<string> Indicators { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
        public Dictionary<string, decimal> FactorScores { get; set; } = new();
        public DateTime AssessedAt { get; set; }
    }

    

    public class BiometricVerificationResultDto
    {
        public bool IsMatch { get; set; }
        public decimal MatchScore { get; set; }
        public string Message { get; set; } = string.Empty;
    }

  

 

   
    public class SanctionsScreeningRequestDto
    {
        public string CustomerId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? DateOfBirth { get; set; }
        public string? Country { get; set; }
    }

    public class BatchScreeningResultDto
    {
        public int TotalProcessed { get; set; }
        public int HitsFound { get; set; }
        public int Clear { get; set; }
        public List<SanctionsScreeningResponseDto> Results { get; set; } = new();
    }

    
    public class IdentityValidationResultDto
    {
        public bool IsValid { get; set; }
        public List<string> ValidationErrors { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
        public Dictionary<string, bool> AttributeChecks { get; set; } = new();
    }
}
