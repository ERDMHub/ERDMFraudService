using ERDM.Fraud.Service.Domain.Enums;
using ERDM.Fraud.Service.Domain.Events;

namespace ERDM.Fraud.Service.Infrastructure.Services
{
    public class SanctionsScreeningService : ISanctionsScreeningService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<SanctionsScreeningService> _logger;
        private readonly IConfiguration _configuration;
        private List<SanctionsList> _cachedLists;

        public SanctionsScreeningService(
            IHttpClientFactory httpClientFactory,
            ILogger<SanctionsScreeningService> logger,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
            _cachedLists = new List<SanctionsList>();
        }

        public async Task<SanctionsScreeningResult> ScreenCustomer(string fullName, string? dateOfBirth, string? country, List<string> listsToScreen)
        {
            try
            {
                _logger.LogInformation("Screening customer {FullName} against sanctions lists", fullName);

                // In production, this would call sanctions screening APIs like:
                // - Dow Jones Risk & Compliance
                // - Refinitiv World-Check
                // - LexisNexis Bridger
                // - OFAC SDN List

                await Task.Delay(200); // Simulate API call

                var hits = new List<SanctionHit>();
                var random = new Random();

                // Simulate screening against different lists
                foreach (var listName in listsToScreen)
                {
                    var hitScore = random.Next(0, 100);

                    if (hitScore > 85) // 15% chance of hit for demo
                    {
                        hits.Add(new SanctionHit
                        {
                            ListName = listName,
                            Name = fullName,
                            MatchType = hitScore > 95 ? "Exact" : (hitScore > 90 ? "Fuzzy" : "Partial"),
                            MatchScore = hitScore,
                            MatchedAt = DateTime.UtcNow
                        });
                    }
                }

                var status = hits.Any() ? ScreeningStatus.Hit : ScreeningStatus.Clear;

                // For high-profile matches, mark as confirmed
                if (hits.Any(h => h.MatchScore > 95))
                {
                    status = ScreeningStatus.ConfirmedMatch;
                }

                return new SanctionsScreeningResult
                {
                    CustomerId = fullName,
                    Status = status,
                    Hits = hits,
                    ScreeningReference = $"SCR-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid():N}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error screening customer {FullName}", fullName);
                return new SanctionsScreeningResult
                {
                    Status = ScreeningStatus.Pending,
                    Hits = new List<SanctionHit>(),
                    ScreeningReference = null
                };
            }
        }

        public async Task<List<SanctionsList>> GetAvailableSanctionsLists()
        {
            if (_cachedLists.Any())
                return _cachedLists;

            // In production, fetch from configuration or API
            await Task.Delay(50);

            _cachedLists = new List<SanctionsList>
            {
                new SanctionsList { ListId = "OFAC", ListName = "OFAC SDN List", ListType = "US Sanctions", TotalEntries = 5000, LastUpdated = DateTime.UtcNow.AddDays(-1) },
                new SanctionsList { ListId = "UN", ListName = "UN Security Council", ListType = "UN Sanctions", TotalEntries = 800, LastUpdated = DateTime.UtcNow.AddDays(-2) },
                new SanctionsList { ListId = "EU", ListName = "EU Consolidated List", ListType = "EU Sanctions", TotalEntries = 2000, LastUpdated = DateTime.UtcNow.AddDays(-3) },
                new SanctionsList { ListId = "UK", ListName = "UK Sanctions List", ListType = "UK Sanctions", TotalEntries = 1200, LastUpdated = DateTime.UtcNow.AddDays(-1) },
                new SanctionsList { ListId = "INTERPOL", ListName = "INTERPOL Red Notices", ListType = "International", TotalEntries = 7000, LastUpdated = DateTime.UtcNow.AddDays(-7) }
            };

            return _cachedLists;
        }

        public async Task<SanctionsScreeningResult> ScreenBatchAsync(List<SanctionsScreeningRequest> requests)
        {
            // For batch processing, would typically queue and process asynchronously
            _logger.LogInformation("Processing batch sanctions screening for {Count} customers", requests.Count);

            var consolidatedResult = new SanctionsScreeningResult
            {
                Status = ScreeningStatus.Clear,
                Hits = new List<SanctionHit>()
            };

            foreach (var request in requests)
            {
                var result = await ScreenCustomer(request.FullName, request.DateOfBirth, request.Country,
                    new List<string> { "OFAC", "UN", "EU" });

                if (result.Status != ScreeningStatus.Clear)
                {
                    consolidatedResult.Status = ScreeningStatus.Hit;
                    consolidatedResult.Hits.AddRange(result.Hits);
                }
            }

            return consolidatedResult;
        }
    }
}
