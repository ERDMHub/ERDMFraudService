namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class FraudStatisticsDto
    {
        public int TotalFraudCases { get; set; }
        public int OpenCases { get; set; }
        public int ClosedCases { get; set; }
        public Dictionary<string, int> CasesByType { get; set; } = new();
        public Dictionary<string, int> CasesByStatus { get; set; } = new();
        public int HighRiskDevices { get; set; }
        public int BlacklistedDevices { get; set; }
        public int SyntheticIdentitiesDetected { get; set; }
        public int SanctionsHits { get; set; }
        public decimal AverageRiskScore { get; set; }
        public List<FraudTrendDto> FraudTrend { get; set; } = new();
        public DateTime AsOfDate { get; set; }
    }
}
