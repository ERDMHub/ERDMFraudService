namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class FraudNetworkEdgeDto
    {
        public string Source { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string Relationship { get; set; } = string.Empty;
        public decimal? Weight { get; set; }
    }
}
