namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class FraudNetworkNodeDto
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public int RiskScore { get; set; }
        public Dictionary<string, object>? Attributes { get; set; }
    }
}
