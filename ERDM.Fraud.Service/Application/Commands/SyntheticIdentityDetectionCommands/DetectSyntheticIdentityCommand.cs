using ERDM.Fraud.Application.Common;
using ERDM.Fraud.Service.Contracts.Dtos;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class DetectSyntheticIdentityCommand : CommandBase<SyntheticIdentityDetectionResultDto>
    {
        public string CustomerId { get; set; } = string.Empty;
        public CustomerProfileDataDto ProfileData { get; set; } = new();
    }
}
