namespace ERDM.Fraud.Service.Infrastructure.ReadModels
{
    public class FraudCaseTimelineReadModel
    {
        public string EventId { get; set; } = string.Empty;
        public string EventType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
        public string PerformedBy { get; set; } = string.Empty;
        public Dictionary<string, object> Details { get; set; } = new();
    }

}
