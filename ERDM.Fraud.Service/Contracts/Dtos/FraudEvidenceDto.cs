namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class FraudEvidenceDto
    {
        public string EvidenceId { get; set; } = string.Empty;
        public string EvidenceType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CollectedAt { get; set; }
        public string CollectedBy { get; set; } = string.Empty;
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}
