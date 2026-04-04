using ERDM.Core.Entities;
using MediatR;

namespace ERDM.Fraud.Service.Domain.Events
{
    // Base class for all MediatR notifications
    public abstract class FraudDomainEventBase : DomainEventBase, INotification
    {
        protected FraudDomainEventBase()
        {
            EventId = Guid.NewGuid().ToString();
            OccurredOn = DateTime.UtcNow;
        }

        public string EventId { get; }
        public DateTime OccurredOn { get; }
    }
}
