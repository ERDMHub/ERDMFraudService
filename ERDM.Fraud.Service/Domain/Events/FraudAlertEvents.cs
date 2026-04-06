using ERDM.Core.Entities;
using ERDM.Fraud.Service.Domain.Entities;
using MediatR;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class FraudAlertCreatedEvent : DomainEventBase, INotification
    {
        public FraudAlertCreatedEvent(FraudAlert alert)
        {
            EntityId = alert.Id;
            EntityType = nameof(FraudAlert);
            AlertId = alert.AlertId;
            CustomerId = alert.CustomerId;
            AlertType = alert.AlertType;
            Severity = alert.Severity;
            Description = alert.Description;
            CreatedAt = alert.CreatedAt.Value;
        }

        public string AlertId { get; }
        public string CustomerId { get; }
        public string AlertType { get; }
        public string Severity { get; }
        public string Description { get; }
        public DateTime CreatedAt { get; }
    }

    public class FraudAlertResolvedEvent : DomainEventBase, INotification
    {
        public FraudAlertResolvedEvent(FraudAlert alert)
        {
            EntityId = alert.Id;
            EntityType = nameof(FraudAlert);
            AlertId = alert.AlertId;
            ResolvedAt = alert.ResolvedAt ?? DateTime.UtcNow;
        }

        public string AlertId { get; }
        public DateTime ResolvedAt { get; }
    }
}
