namespace ERDM.Fraud.Service.Infrastructure.ReadModels
{
    public class SanctionHitReadModel
    {
        public string ListName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MatchType { get; set; } = string.Empty;
        public decimal MatchScore { get; set; }
        public DateTime MatchedAt { get; set; }
        public string? ReferenceNumber { get; set; }
    }
}
