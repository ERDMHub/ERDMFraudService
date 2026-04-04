using ERDM.Fraud.Application.Common;
using ERDM.Fraud.Service.Contracts.Dtos;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class ProcessLivenessDetectionCommand : CommandBase<LivenessResultDto>
    {
        public string VerificationId { get; set; } = string.Empty;
        public string VideoUrl { get; set; } = string.Empty;
    }
}
