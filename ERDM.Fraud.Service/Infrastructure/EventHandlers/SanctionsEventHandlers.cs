using ERDM.Fraud.Service.Domain.Enums;
using ERDM.Fraud.Service.Domain.Events;
using MediatR;

namespace ERDM.Fraud.Service.Infrastructure.EventHandlers
{
    public class SanctionsEventHandlers : INotificationHandler<SanctionsScreeningCompletedEvent>
    {
        private readonly ILogger<SanctionsEventHandlers> _logger;

        public SanctionsEventHandlers(ILogger<SanctionsEventHandlers> logger)
        {
            _logger = logger;
        }

        public Task Handle(SanctionsScreeningCompletedEvent notification, CancellationToken cancellationToken)
        {
            if (notification.Status == ScreeningStatus.Hit || notification.Status == ScreeningStatus.ConfirmedMatch)
            {
                _logger.LogWarning("Sanctions hit for customer {CustomerId}, Status: {Status}, Hits: {HitCount}",
                    notification.CustomerId, notification.Status, notification.Hits.Count);

                foreach (var hit in notification.Hits)
                {
                    _logger.LogWarning("  - List: {List}, Name: {Name}, Match: {MatchType}, Score: {Score}",
                        hit.ListName, hit.Name, hit.MatchType, hit.MatchScore);
                }
            }
            else
            {
                _logger.LogInformation("Sanctions screening completed for {CustomerId}, Status: {Status}",
                    notification.CustomerId, notification.Status);
            }

            return Task.CompletedTask;
        }
    }
}
