namespace ERDM.Fraud.Service.Domain.ValueObjects
{
    public class LivenessResult
    {
        public bool IsAlive { get; set; }
        public decimal ConfidenceScore { get; set; }
        public string Method { get; set; } = string.Empty;
        public List<string> ChecksPassed { get; set; } = new();
        public List<string> ChecksFailed { get; set; } = new();
    }
}
