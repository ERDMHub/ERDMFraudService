namespace ERDM.Fraud.Service.Domain.ValueObjects
{
    public class FraudResolution
    {
        public string ResolutionType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime ResolvedAt { get; set; }
        public string ResolvedBy { get; set; } = string.Empty;
        public string? ActionTaken { get; set; }
    }

}
