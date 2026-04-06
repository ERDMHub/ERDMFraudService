using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class CreateFraudAlertCommand : IRequest<ApiResponse<FraudAlertResponseDto>>
    {
        public string CustomerId { get; set; }
        public string AlertType { get; set; }
        public string Severity { get; set; }
        public string Description { get; set; }
    }
}
