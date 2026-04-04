using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class UpdateFraudCaseRiskScoreCommand : IRequest<ApiResponse<bool>>
    {
        public string CaseId { get; set; } = string.Empty;
        public int NewRiskScore { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
    }
}
