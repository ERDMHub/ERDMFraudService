namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class SanctionHitDto
    {
        public string ListName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string MatchType { get; set; } = string.Empty;
        public decimal MatchScore { get; set; }
        public DateTime MatchedAt { get; set; }
    }
}
