namespace ERDM.Fraud.Service.Infrastructure.ReadModels
{
    public class FraudIndicatorReadModel
    {
        public string IndicatorId { get; set; } = string.Empty;
        public string IndicatorName { get; set; } = string.Empty;
        public string IndicatorType { get; set; } = string.Empty;
        public decimal Weight { get; set; }
        public string Evidence { get; set; } = string.Empty;
        public DateTime DetectedAt { get; set; }
        public bool IsConfirmed { get; set; }
    }

}
