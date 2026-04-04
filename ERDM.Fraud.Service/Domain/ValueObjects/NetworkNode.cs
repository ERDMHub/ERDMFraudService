namespace ERDM.Fraud.Service.Domain.ValueObjects
{
    public class NetworkNode
    {
        public string NodeId { get; set; } = string.Empty;
        public string NodeType { get; set; } = string.Empty;
        public decimal ConnectionStrength { get; set; }
        public List<string> Connections { get; set; } = new();
    }
}
