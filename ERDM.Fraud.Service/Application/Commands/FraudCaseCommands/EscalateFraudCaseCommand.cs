using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class EscalateFraudCaseCommand : IRequest<ApiResponse<bool>>
    {
        public string CaseId { get; set; } = string.Empty;
        public string EscalateTo { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string EscalatedBy { get; set; } = string.Empty;
    }
}
