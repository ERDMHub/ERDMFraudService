using ERDM.Fraud.Application.Common;
using ERDM.Fraud.Service.Contracts.Dtos;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class OpenFraudCaseCommand : CommandBase<FraudCaseResponseDto>
    {
        public string CustomerId { get; set; } = string.Empty;
        public string FraudType { get; set; } = string.Empty;
        public int RiskScore { get; set; }
        public string? Description { get; set; }
    }
}
