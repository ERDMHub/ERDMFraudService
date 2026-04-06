namespace ERDM.Fraud.Service.Infrastructure.ReadModels
{
    public class FraudAlertReadModel
    {
        public string Id { get; set; }
        public string AlertId { get; set; }
        public string CustomerId { get; set; }
        public string AlertType { get; set; }
        public string Severity { get; set; }
        public string Description { get; set; }
        public bool IsResolved { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }
}
