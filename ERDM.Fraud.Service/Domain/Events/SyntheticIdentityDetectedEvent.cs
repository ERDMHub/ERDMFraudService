namespace ERDM.Fraud.Service.Domain.Events
{
    // Synthetic Identity Detection Events
    public class SyntheticIdentityDetectedEvent : FraudDomainEventBase
    {
        public SyntheticIdentityDetectedEvent(string customerId, decimal confidenceScore, List<string> indicators)
        {
            EntityId = Guid.NewGuid().ToString();
            EntityType = "SyntheticIdentity";
            CustomerId = customerId;
            ConfidenceScore = confidenceScore;
            Indicators = indicators;
            DetectedAt = DateTime.UtcNow;
        }

        public string CustomerId { get; }
        public decimal ConfidenceScore { get; }
        public List<string> Indicators { get; }
        public DateTime DetectedAt { get; }
    }
}
