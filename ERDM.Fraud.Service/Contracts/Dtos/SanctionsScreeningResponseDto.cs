namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class SanctionsScreeningResponseDto
    {
        public string ScreeningId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public List<SanctionHitDto> Hits { get; set; } = new();
        public DateTime ScreenedAt { get; set; }
    }
}
