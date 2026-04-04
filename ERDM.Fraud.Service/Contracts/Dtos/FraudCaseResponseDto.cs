namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class FraudCaseResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string CaseId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string FraudType { get; set; } = string.Empty;
        public int RiskScore { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<FraudEvidenceDto> Evidence { get; set; } = new();
        public string? AssignedTo { get; set; }
        public FraudResolutionDto? Resolution { get; set; }
        public List<string> LinkedCases { get; set; } = new();
        public List<FraudNetworkNodeDto> NetworkNodes { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }

}
