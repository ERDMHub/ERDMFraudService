using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Service.Application.Queries
{
    public class GetFraudAlertByIdQuery : IRequest<ApiResponse<FraudAlertResponseDto>>
    {
        public string AlertId { get; set; }
    }

    // Query to get all alerts for a customer
    public class GetFraudAlertsByCustomerQuery : IRequest<ApiResponse<List<FraudAlertResponseDto>>>
    {
        public string CustomerId { get; set; }
    }

    // Query to get unresolved alerts
    public class GetUnresolvedAlertsQuery : IRequest<ApiResponse<List<FraudAlertResponseDto>>>
    {
        public string Severity { get; set; } // Optional filter
    }

    public class GetAllFraudAlertsQuery : IRequest<ApiResponse<List<FraudAlertResponseDto>>>
    {
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
