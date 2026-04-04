using ERDM.Fraud.Application.Common;
using ERDM.Fraud.Service.Contracts.Dtos;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class CompleteIdentityVerificationCommand : CommandBase<IdentityVerificationResponseDto>
    {
        public string VerificationId { get; set; } = string.Empty;
        public int VerificationScore { get; set; }
        public VerificationDetailsDto Details { get; set; } = new();
        public string VerifiedBy { get; set; } = string.Empty;
    }

}
