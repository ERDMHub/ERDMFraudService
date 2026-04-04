namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class SyntheticIdentityDetectionResultDto
    {
        public bool IsSynthetic { get; set; }
        public decimal ConfidenceScore { get; set; }
        public List<string> Indicators { get; set; } = new();
        public List<string> Recommendations { get; set; } = new();
    }
}
