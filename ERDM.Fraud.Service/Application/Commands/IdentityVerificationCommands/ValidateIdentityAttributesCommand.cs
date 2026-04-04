using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class ValidateIdentityAttributesCommand : IRequest<ApiResponse<IdentityValidationResultDto>>
    {
        public CustomerProfileDataDto ProfileData { get; set; } = new();
    }

}
