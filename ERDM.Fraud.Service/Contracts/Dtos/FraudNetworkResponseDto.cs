namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class FraudNetworkResponseDto
    {
        public List<FraudNetworkNodeDto> Nodes { get; set; } = new();
        public List<FraudNetworkEdgeDto> Edges { get; set; } = new();
    }
}
