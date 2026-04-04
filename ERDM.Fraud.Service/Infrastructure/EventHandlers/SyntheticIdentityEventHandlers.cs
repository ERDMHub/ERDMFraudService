using ERDM.Fraud.Service.Domain.Events;
using MediatR;

namespace ERDM.Fraud.Service.Infrastructure.EventHandlers
{
    public class SyntheticIdentityEventHandlers : INotificationHandler<SyntheticIdentityDetectedEvent>
    {
        private readonly ILogger<SyntheticIdentityEventHandlers> _logger;

        public SyntheticIdentityEventHandlers(ILogger<SyntheticIdentityEventHandlers> logger)
        {
            _logger = logger;
        }

        public Task Handle(SyntheticIdentityDetectedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Synthetic identity detected for customer {CustomerId}, Confidence: {Confidence}, Indicators: {Indicators}",
                notification.CustomerId, notification.ConfidenceScore, string.Join(", ", notification.Indicators));

            return Task.CompletedTask;
        }
    }
}
