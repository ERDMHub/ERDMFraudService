using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class ResolveFraudAlertCommand : IRequest<ApiResponse<bool>>
    {
        public string AlertId { get; set; }
    }
}
