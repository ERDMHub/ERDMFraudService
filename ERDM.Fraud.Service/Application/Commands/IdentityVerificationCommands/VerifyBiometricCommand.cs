using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Service.Application.Commands
{
    // Additional command DTOs
    public class VerifyBiometricCommand : IRequest<ApiResponse<BiometricVerificationResultDto>>
    {
        public string CustomerId { get; set; } = string.Empty;
        public string BiometricData { get; set; } = string.Empty;
        public string BiometricType { get; set; } = string.Empty;
    }
}
