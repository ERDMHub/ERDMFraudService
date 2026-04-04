using ERDM.Fraud.Application.Common;
using ERDM.Fraud.Service.Contracts.Dtos;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class InitiateIdentityVerificationCommand : CommandBase<IdentityVerificationResponseDto>
    {
        public string CustomerId { get; set; } = string.Empty;
        public string DocumentType { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string DocumentImageUrl { get; set; } = string.Empty;
        public string SelfieImageUrl { get; set; } = string.Empty;
    }
}
