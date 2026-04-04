using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Application.Common
{
    public abstract class CommandBase<TResponse> : IRequest<ApiResponse<TResponse>>
    {
        public string CommandId { get; } = Guid.NewGuid().ToString();
        public DateTime Timestamp { get; } = DateTime.UtcNow;
        public string? CorrelationId { get; set; }
        public string? UserId { get; set; }
    }
}