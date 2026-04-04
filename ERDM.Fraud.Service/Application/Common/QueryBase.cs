using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Service.Application.Common
{
    public abstract class QueryBase<TResponse> : IRequest<ApiResponse<TResponse>>
    {
        public string QueryId { get; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; } = DateTime.UtcNow;
        public string? CorrelationId { get; set; }
    }
}
