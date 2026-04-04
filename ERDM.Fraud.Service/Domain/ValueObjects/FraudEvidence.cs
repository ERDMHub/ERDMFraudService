namespace ERDM.Fraud.Service.Domain.ValueObjects
{
    public class FraudEvidence
    {
        public string EvidenceId { get; set; } = Guid.NewGuid().ToString();
        public string EvidenceType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CollectedAt { get; set; }
        public string CollectedBy { get; set; } = string.Empty;
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}
