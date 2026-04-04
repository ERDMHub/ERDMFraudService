namespace ERDM.Fraud.Service.Infrastructure.ReadModels
{
    public class FraudRiskAssessmentReadModel
    {
        public decimal OverallRiskScore { get; set; }
        public string OverallRiskLevel { get; set; } = string.Empty;
        public decimal ProbabilityOfFraud { get; set; }
        public decimal PotentialLossAmount { get; set; }
        public Dictionary<string, decimal> RiskFactors { get; set; } = new();
        public List<string> RecommendedActions { get; set; } = new();
        public DateTime AssessedAt { get; set; }
        public string AssessedBy { get; set; } = string.Empty;
    }
}
