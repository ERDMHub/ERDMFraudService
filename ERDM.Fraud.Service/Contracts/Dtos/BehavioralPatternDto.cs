namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class BehavioralPatternDto
    {
        public string PatternType { get; set; } = string.Empty;
        public string PatternValue { get; set; } = string.Empty;
        public DateTime DetectedAt { get; set; }
        public decimal Confidence { get; set; }
    }
}
