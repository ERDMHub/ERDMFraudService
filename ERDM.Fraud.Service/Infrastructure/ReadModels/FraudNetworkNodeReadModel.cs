namespace ERDM.Fraud.Service.Infrastructure.ReadModels
{
    public class FraudNetworkNodeReadModel
    {
        public string NodeId { get; set; } = string.Empty;
        public string NodeType { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public int RiskScore { get; set; }
        public decimal ConnectionStrength { get; set; }
        public List<string> Connections { get; set; } = new();
        public DateTime ConnectedAt { get; set; }
    }

}
