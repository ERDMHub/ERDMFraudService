using ERDM.Fraud.Service.Domain.Common;
using ERDM.Fraud.Service.Domain.Events;

namespace ERDM.Fraud.Service.Domain.Entities
{
    public class FraudAlert : AggregateRoot
    {
        public string AlertId { get; private set; }
        public string CustomerId { get; private set; }
        public string AlertType { get; private set; }
        public string Severity { get; private set; }
        public string Description { get; private set; }
        public bool IsResolved { get; private set; }
        public DateTime? ResolvedAt { get; private set; }

        // Parameterless constructor for MongoDB
        public FraudAlert() { }

        // Factory method
        public static FraudAlert Create(string customerId, string alertType, string severity, string description)
        {
            var alert = new FraudAlert
            {
                AlertId = $"ALT-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid():N}".Substring(0, 20),
                CustomerId = customerId,
                AlertType = alertType,
                Severity = severity,
                Description = description,
                IsResolved = false,
                ResolvedAt = null
            };

            // Set BaseEntity properties
            alert.CreatedAt = DateTime.UtcNow;
            alert.CreatedBy = "system";
            alert.IsActive = true;
            alert.Version = 1;

            // Raise domain event
            alert.AddDomainEvent(new FraudAlertCreatedEvent(alert));

            return alert;
        }

        public void Resolve()
        {
            IsResolved = true;
            ResolvedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;

            // Raise domain event
            AddDomainEvent(new FraudAlertResolvedEvent(this));
        }
    }
}
