namespace ERDM.Fraud.Service.Domain.Events
{
    // Network Analysis Events
    public class FraudRingDetectedEvent : FraudDomainEventBase
    {
        public FraudRingDetectedEvent(string ringId, List<string> customerIds, decimal confidenceScore)
        {
            EntityId = ringId;
            EntityType = "FraudRing";
            RingId = ringId;
            CustomerIds = customerIds;
            ConfidenceScore = confidenceScore;
            DetectedAt = DateTime.UtcNow;
        }

        public string RingId { get; }
        public List<string> CustomerIds { get; }
        public decimal ConfidenceScore { get; }
        public DateTime DetectedAt { get; }
    }
}
