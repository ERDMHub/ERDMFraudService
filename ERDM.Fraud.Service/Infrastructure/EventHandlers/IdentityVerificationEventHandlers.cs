using ERDM.Fraud.Service.Domain.Events;
using MediatR;

namespace ERDM.Fraud.Service.Infrastructure.EventHandlers
{
    public class IdentityVerificationEventHandlers :
       INotificationHandler<IdentityVerificationInitiatedEvent>,
       INotificationHandler<IdentityVerificationCompletedEvent>,
       INotificationHandler<LivenessDetectionCompletedEvent>,
       INotificationHandler<BiometricRegisteredEvent>
    {
        private readonly ILogger<IdentityVerificationEventHandlers> _logger;

        public IdentityVerificationEventHandlers(ILogger<IdentityVerificationEventHandlers> logger)
        {
            _logger = logger;
        }

        public Task Handle(IdentityVerificationInitiatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Identity verification initiated for customer {CustomerId}, VerificationId: {VerificationId}",
                notification.CustomerId, notification.VerificationId);

            return Task.CompletedTask;
        }

        public Task Handle(IdentityVerificationCompletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Identity verification completed for customer {CustomerId}, Status: {Status}, Score: {Score}",
                notification.CustomerId, notification.Status, notification.Score);

            return Task.CompletedTask;
        }

        public Task Handle(LivenessDetectionCompletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Liveness detection completed for {VerificationId}, IsAlive: {IsAlive}, Confidence: {Confidence}",
                notification.VerificationId, notification.IsAlive, notification.ConfidenceScore);

            return Task.CompletedTask;
        }

        public Task Handle(BiometricRegisteredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Biometric registered for customer {CustomerId}", notification.CustomerId);

            return Task.CompletedTask;
        }
    }
}
