namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class LivenessResultDto
    {
        public bool IsAlive { get; set; }
        public decimal ConfidenceScore { get; set; }
        public string Method { get; set; } = string.Empty;
        public List<string> ChecksPassed { get; set; } = new();
        public List<string> ChecksFailed { get; set; } = new();
    }

}
