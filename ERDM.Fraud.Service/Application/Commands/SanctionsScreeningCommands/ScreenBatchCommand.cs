using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class ScreenBatchCommand : IRequest<ApiResponse<BatchScreeningResultDto>>
    {
        public List<SanctionsScreeningRequestDto> Requests { get; set; } = new();
    }

}
