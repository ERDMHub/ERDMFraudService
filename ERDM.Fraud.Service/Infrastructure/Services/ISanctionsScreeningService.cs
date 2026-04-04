using ERDM.Fraud.Service.Domain.Enums;
using ERDM.Fraud.Service.Domain.Events;

namespace ERDM.Fraud.Service.Infrastructure.Services
{
    public interface ISanctionsScreeningService
    {
        Task<SanctionsScreeningResult> ScreenCustomer(string fullName, string? dateOfBirth, string? country, List<string> listsToScreen);
        Task<List<SanctionsList>> GetAvailableSanctionsLists();
        Task<SanctionsScreeningResult> ScreenBatchAsync(List<SanctionsScreeningRequest> requests);
    }

    public class SanctionsScreeningRequest
    {
        public string CustomerId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? DateOfBirth { get; set; }
        public string? Country { get; set; }
    }

    public class SanctionsScreeningResult
    {
        public string ScreeningId { get; set; } = Guid.NewGuid().ToString();
        public string CustomerId { get; set; } = string.Empty;
        public ScreeningStatus Status { get; set; }
        public List<SanctionHit> Hits { get; set; } = new();
        public DateTime ScreenedAt { get; set; } = DateTime.UtcNow;
        public string? ScreeningReference { get; set; }
    }

    public class SanctionsList
    {
        public string ListId { get; set; } = string.Empty;
        public string ListName { get; set; } = string.Empty;
        public string ListType { get; set; } = string.Empty;
        public int TotalEntries { get; set; }
        public DateTime LastUpdated { get; set; }
    }

   
}
