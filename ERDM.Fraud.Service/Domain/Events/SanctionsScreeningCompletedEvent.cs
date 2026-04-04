using ERDM.Fraud.Service.Domain.Enums;

namespace ERDM.Fraud.Service.Domain.Events
{
    // Sanctions Screening Events
    public class SanctionsScreeningCompletedEvent : FraudDomainEventBase
    {
        public SanctionsScreeningCompletedEvent(string customerId, ScreeningStatus status, List<SanctionHit> hits)
        {
            EntityId = Guid.NewGuid().ToString();
            EntityType = "SanctionsScreening";
            CustomerId = customerId;
            Status = status;
            Hits = hits;
            CompletedAt = DateTime.UtcNow;
        }

        public string CustomerId { get; }
        public ScreeningStatus Status { get; }
        public List<SanctionHit> Hits { get; }
        public DateTime CompletedAt { get; }
    }
}
