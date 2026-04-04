using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class AssignFraudCaseCommand : IRequest<ApiResponse<bool>>
    {
        public string CaseId { get; set; } = string.Empty;
        public string AssigneeId { get; set; } = string.Empty;
        public string AssigneeName { get; set; } = string.Empty;
    }
}
