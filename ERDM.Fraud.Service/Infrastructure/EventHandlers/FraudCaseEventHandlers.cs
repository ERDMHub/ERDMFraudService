using ERDM.Fraud.Service.Domain.Events;
using MediatR;

namespace ERDM.Fraud.Service.Infrastructure.EventHandlers
{
    public class FraudCaseEventHandlers :
        INotificationHandler<FraudCaseOpenedEvent>,
        INotificationHandler<FraudEvidenceAddedEvent>,
        INotificationHandler<FraudCaseLinkedEvent>,
        INotificationHandler<FraudCaseResolvedEvent>,
        INotificationHandler<FraudRingDetectedEvent>
    {
        private readonly ILogger<FraudCaseEventHandlers> _logger;

        public FraudCaseEventHandlers(ILogger<FraudCaseEventHandlers> logger)
        {
            _logger = logger;
        }

        public Task Handle(FraudCaseOpenedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Fraud case opened: {CaseId}, Customer: {CustomerId}, Type: {FraudType}, RiskScore: {RiskScore}",
                notification.CaseId, notification.CustomerId, notification.FraudType, notification.RiskScore);

            return Task.CompletedTask;
        }

        public Task Handle(FraudEvidenceAddedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Evidence added to case {CaseId}: {EvidenceType}",
                notification.CaseId, notification.EvidenceType);

            return Task.CompletedTask;
        }

        public Task Handle(FraudCaseLinkedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Case {CaseId} linked to case {LinkedCaseId}",
                notification.CaseId, notification.LinkedCaseId);

            return Task.CompletedTask;
        }

        public Task Handle(FraudCaseResolvedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fraud case resolved: {CaseId}, Resolution: {ResolutionType}, Action: {ActionTaken}",
                notification.CaseId, notification.ResolutionType, notification.ActionTaken);

            return Task.CompletedTask;
        }

        public Task Handle(FraudRingDetectedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Fraud ring detected: {RingId}, Customers: {CustomerCount}, Confidence: {Confidence}",
                notification.RingId, notification.CustomerIds.Count, notification.ConfidenceScore);

            return Task.CompletedTask;
        }
    }
}
