using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using ERDM.Fraud.Service.Infrastructure.ReadModels;
using MediatR;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Application.Queries.Handlers
{
    public class GetFraudDashboardSummaryQueryHandler : IRequestHandler<GetFraudDashboardSummaryQuery, ApiResponse<FraudDashboardSummaryDto>>
    {
        private readonly IMongoCollection<FraudCaseReadModel> _fraudCaseCollection;
        private readonly IMongoCollection<DeviceFingerprintReadModel> _deviceCollection;
        private readonly IMongoCollection<IdentityVerificationReadModel> _verificationCollection;
        private readonly ILogger<GetFraudDashboardSummaryQueryHandler> _logger;

        public GetFraudDashboardSummaryQueryHandler(
            IMongoDatabase database,
            ILogger<GetFraudDashboardSummaryQueryHandler> logger)
        {
            _fraudCaseCollection = database.GetCollection<FraudCaseReadModel>("fraud_cases_read");
            _deviceCollection = database.GetCollection<DeviceFingerprintReadModel>("device_fingerprints_read");
            _verificationCollection = database.GetCollection<IdentityVerificationReadModel>("identity_verifications_read");
            _logger = logger;
        }

        public async Task<ApiResponse<FraudDashboardSummaryDto>> Handle(GetFraudDashboardSummaryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var fraudCases = await _fraudCaseCollection.Find(_ => true).ToListAsync(cancellationToken);
                var devices = await _deviceCollection.Find(_ => true).ToListAsync(cancellationToken);
                var verifications = await _verificationCollection.Find(_ => true).ToListAsync(cancellationToken);

                var fraudTypeBreakdown = fraudCases
                    .GroupBy(f => f.FraudType)
                    .Select(g => new FraudTypeSummaryDto
                    {
                        FraudType = g.Key,
                        Count = g.Count(),
                        Percentage = (decimal)g.Count() / fraudCases.Count * 100
                    })
                    .ToList();

                var last7Days = fraudCases
                    .Where(f => f.OpenedAt >= DateTime.UtcNow.AddDays(-7))
                    .GroupBy(f => f.OpenedAt.Date)
                    .Select(g => new DailyFraudSummaryDto
                    {
                        Date = g.Key,
                        CasesCount = g.Count(),
                        HighRiskCount = g.Count(f => f.RiskScore >= 60)
                    })
                    .OrderBy(d => d.Date)
                    .ToList();

                var dashboard = new FraudDashboardSummaryDto
                {
                    TotalFraudCases = fraudCases.Count,
                    OpenCases = fraudCases.Count(f => f.Status == "Open"),
                    UnderReview = fraudCases.Count(f => f.Status == "UnderReview"),
                    ClosedCases = fraudCases.Count(f => f.Status == "Closed"),
                    EscalatedCases = fraudCases.Count(f => f.Status == "Escalated"),
                    HighRiskDevices = devices.Count(d => d.RiskScore >= 60),
                    BlacklistedDevices = devices.Count(d => d.IsBlacklisted),
                    TotalDevices = devices.Count,
                    PendingVerifications = verifications.Count(v => v.VerificationStatus == "Pending"),
                    ApprovedVerifications = verifications.Count(v => v.VerificationStatus == "Approved"),
                    RejectedVerifications = verifications.Count(v => v.VerificationStatus == "Rejected"),
                    SyntheticIdentitiesDetected = fraudCases.Count(f => f.FraudType == "SyntheticIdentity"),
                    SanctionsHits = fraudCases.Count(f => f.HasSanctionsHit),
                    TotalPotentialLoss = fraudCases.Sum(f => f.RiskAssessment?.PotentialLossAmount ?? 0),
                    FraudTypeBreakdown = fraudTypeBreakdown,
                    Last7Days = last7Days,
                    AsOfDate = DateTime.UtcNow
                };

                return ApiResponse<FraudDashboardSummaryDto>.Ok(dashboard);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting fraud dashboard summary");
                return ApiResponse<FraudDashboardSummaryDto>.Fail(ex.Message);
            }
        }
    }

    public class GetFraudTrendsQueryHandler : IRequestHandler<GetFraudTrendsQuery, ApiResponse<FraudTrendsDto>>
    {
        private readonly IMongoCollection<FraudCaseReadModel> _collection;
        private readonly ILogger<GetFraudTrendsQueryHandler> _logger;

        public GetFraudTrendsQueryHandler(IMongoDatabase database, ILogger<GetFraudTrendsQueryHandler> logger)
        {
            _collection = database.GetCollection<FraudCaseReadModel>("fraud_cases_read");
            _logger = logger;
        }

        public async Task<ApiResponse<FraudTrendsDto>> Handle(GetFraudTrendsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var startDate = DateTime.UtcNow.AddMonths(-request.Months);
                var fraudCases = await _collection
                    .Find(f => f.OpenedAt >= startDate)
                    .ToListAsync(cancellationToken);

                var monthlyTrends = fraudCases
                    .GroupBy(f => new { f.OpenedAt.Year, f.OpenedAt.Month })
                    .Select(g => new MonthlyFraudTrendDto
                    {
                        Month = g.Key.Month.ToString("00"),
                        Year = g.Key.Year,
                        CasesCount = g.Count(),
                        UniqueCustomers = g.Select(f => f.CustomerId).Distinct().Count()
                    })
                    .OrderBy(m => m.Year).ThenBy(m => m.Month)
                    .ToList();

                var fraudTypeTrends = fraudCases
                    .GroupBy(f => f.FraudType)
                    .Select(g => new FraudTypeTrendDto
                    {
                        FraudType = g.Key,
                        MonthlyCounts = g
                            .GroupBy(f => new { f.OpenedAt.Year, f.OpenedAt.Month })
                            .OrderBy(m => m.Key.Year).ThenBy(m => m.Key.Month)
                            .Select(m => m.Count())
                            .ToList(),
                        GrowthRate = CalculateGrowthRate(g.ToList())
                    })
                    .ToList();

                var riskScoreTrends = fraudCases
                    .GroupBy(f => f.OpenedAt.Date)
                    .Select(g => new RiskScoreTrendDto
                    {
                        Date = g.Key,
                        TotalCases = g.Count()
                    })
                    .OrderBy(r => r.Date)
                    .ToList();

                var trends = new FraudTrendsDto
                {
                    MonthlyTrends = monthlyTrends,
                    FraudTypeTrends = fraudTypeTrends,
                    RiskScoreTrends = riskScoreTrends,
                    NextMonthPrediction = PredictNextMonth(monthlyTrends)
                };

                return ApiResponse<FraudTrendsDto>.Ok(trends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting fraud trends");
                return ApiResponse<FraudTrendsDto>.Fail(ex.Message);
            }
        }

        private decimal CalculateGrowthRate(List<FraudCaseReadModel> cases)
        {
            if (cases.Count < 2) return 0;
            var lastMonth = cases.Count(c => c.OpenedAt >= DateTime.UtcNow.AddMonths(-1));
            var previousMonth = cases.Count(c => c.OpenedAt >= DateTime.UtcNow.AddMonths(-2) && c.OpenedAt < DateTime.UtcNow.AddMonths(-1));
            if (previousMonth == 0) return lastMonth > 0 ? 100 : 0;
            return ((decimal)(lastMonth - previousMonth) / previousMonth) * 100;
        }

        private PredictionDto PredictNextMonth(List<MonthlyFraudTrendDto> trends)
        {
            if (trends.Count < 3) return new PredictionDto { ConfidenceLevel = "Low" };

            var avgGrowth = trends.Skip(1).Average(t => t.CasesCount - trends[0].CasesCount);
            var predictedCases = (int)(trends.Last().CasesCount + avgGrowth);
            var avgRiskScore = trends.Average(t => t.AverageRiskScore);

            return new PredictionDto
            {
                PredictedCases = Math.Max(0, predictedCases),
                PredictedRiskScore = avgRiskScore,
                ConfidenceLevel = trends.Count > 6 ? "High" : "Medium"
            };
        }
    }

    public class GetTopFraudTypesQueryHandler : IRequestHandler<GetTopFraudTypesQuery, ApiResponse<List<TopFraudTypeDto>>>
    {
        private readonly IMongoCollection<FraudCaseReadModel> _collection;
        private readonly ILogger<GetTopFraudTypesQueryHandler> _logger;

        public GetTopFraudTypesQueryHandler(IMongoDatabase database, ILogger<GetTopFraudTypesQueryHandler> logger)
        {
            _collection = database.GetCollection<FraudCaseReadModel>("fraud_cases_read");
            _logger = logger;
        }

        public async Task<ApiResponse<List<TopFraudTypeDto>>> Handle(GetTopFraudTypesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var fraudCases = await _collection.Find(_ => true).ToListAsync(cancellationToken);
                var totalCases = fraudCases.Count;

                var topFraudTypes = fraudCases
                    .GroupBy(f => f.FraudType)
                    .Select(g => new TopFraudTypeDto
                    {
                        FraudType = g.Key,
                        Count = g.Count(),
                        Percentage = (decimal)g.Count() / totalCases * 100,
                        MonthOverMonthChange = CalculateMonthOverMonthChange(g.ToList())
                    })
                    .OrderByDescending(t => t.Count)
                    .Take(request.Limit)
                    .ToList();

                return ApiResponse<List<TopFraudTypeDto>>.Ok(topFraudTypes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting top fraud types");
                return ApiResponse<List<TopFraudTypeDto>>.Fail(ex.Message);
            }
        }

        private decimal CalculateMonthOverMonthChange(List<FraudCaseReadModel> cases)
        {
            var lastMonth = cases.Count(c => c.OpenedAt >= DateTime.UtcNow.AddMonths(-1));
            var previousMonth = cases.Count(c => c.OpenedAt >= DateTime.UtcNow.AddMonths(-2) && c.OpenedAt < DateTime.UtcNow.AddMonths(-1));
            if (previousMonth == 0) return 0;
            return ((decimal)(lastMonth - previousMonth) / previousMonth) * 100;
        }
    }

    public class GetRiskDistributionQueryHandler : IRequestHandler<GetRiskDistributionQuery, ApiResponse<RiskDistributionDto>>
    {
        private readonly IMongoCollection<FraudCaseReadModel> _fraudCollection;
        private readonly IMongoCollection<DeviceFingerprintReadModel> _deviceCollection;
        private readonly ILogger<GetRiskDistributionQueryHandler> _logger;

        public GetRiskDistributionQueryHandler(
            IMongoDatabase database,
            ILogger<GetRiskDistributionQueryHandler> logger)
        {
            _fraudCollection = database.GetCollection<FraudCaseReadModel>("fraud_cases_read");
            _deviceCollection = database.GetCollection<DeviceFingerprintReadModel>("device_fingerprints_read");
            _logger = logger;
        }

        public async Task<ApiResponse<RiskDistributionDto>> Handle(GetRiskDistributionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var fraudCases = await _fraudCollection.Find(_ => true).ToListAsync(cancellationToken);
                var devices = await _deviceCollection.Find(_ => true).ToListAsync(cancellationToken);

                var distribution = new RiskDistributionDto
                {
                    VeryLowRiskCount = fraudCases.Count(f => f.RiskScore < 20),
                    LowRiskCount = fraudCases.Count(f => f.RiskScore >= 20 && f.RiskScore < 40),
                    MediumRiskCount = fraudCases.Count(f => f.RiskScore >= 40 && f.RiskScore < 60),
                    HighRiskCount = fraudCases.Count(f => f.RiskScore >= 60 && f.RiskScore < 80),
                    VeryHighRiskCount = fraudCases.Count(f => f.RiskScore >= 80),
                    RiskByFraudType = fraudCases.GroupBy(f => f.FraudType).ToDictionary(g => g.Key, g => g.Count(f => f.RiskScore >= 60)),
                    RiskByDeviceType = devices.GroupBy(d => d.DeviceType).ToDictionary(g => g.Key, g => g.Count(d => d.RiskScore >= 60))
                };

                return ApiResponse<RiskDistributionDto>.Ok(distribution);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting risk distribution");
                return ApiResponse<RiskDistributionDto>.Fail(ex.Message);
            }
        }
    }

    public class GetDeviceStatisticsQueryHandler : IRequestHandler<GetDeviceStatisticsQuery, ApiResponse<DeviceStatisticsDto>>
    {
        private readonly IMongoCollection<DeviceFingerprintReadModel> _collection;
        private readonly ILogger<GetDeviceStatisticsQueryHandler> _logger;

        public GetDeviceStatisticsQueryHandler(IMongoDatabase database, ILogger<GetDeviceStatisticsQueryHandler> logger)
        {
            _collection = database.GetCollection<DeviceFingerprintReadModel>("device_fingerprints_read");
            _logger = logger;
        }

        public async Task<ApiResponse<DeviceStatisticsDto>> Handle(GetDeviceStatisticsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var devices = await _collection.Find(_ => true).ToListAsync(cancellationToken);

                var riskDistribution = new List<DeviceRiskDistributionDto>
                {
                    new() { RiskLevel = "Very Low", Count = devices.Count(d => d.RiskScore < 20), Percentage = 0 },
                    new() { RiskLevel = "Low", Count = devices.Count(d => d.RiskScore >= 20 && d.RiskScore < 40), Percentage = 0 },
                    new() { RiskLevel = "Medium", Count = devices.Count(d => d.RiskScore >= 40 && d.RiskScore < 60), Percentage = 0 },
                    new() { RiskLevel = "High", Count = devices.Count(d => d.RiskScore >= 60 && d.RiskScore < 80), Percentage = 0 },
                    new() { RiskLevel = "Very High", Count = devices.Count(d => d.RiskScore >= 80), Percentage = 0 }
                };

                var total = riskDistribution.Sum(r => r.Count);
                foreach (var item in riskDistribution)
                {
                    item.Percentage = total > 0 ? (decimal)item.Count / total * 100 : 0;
                }

                var statistics = new DeviceStatisticsDto
                {
                    TotalDevices = devices.Count,
                    UniqueDevices = devices.Select(d => d.DeviceId).Distinct().Count(),
                    BlacklistedDevices = devices.Count(d => d.IsBlacklisted),
                    HighRiskDevices = devices.Count(d => d.RiskScore >= 60),
                    DevicesByType = devices.GroupBy(d => d.DeviceType).ToDictionary(g => g.Key, g => g.Count()),
                    DevicesByOS = devices.GroupBy(d => d.OperatingSystem).ToDictionary(g => g.Key, g => g.Count()),
                    RiskDistribution = riskDistribution
                };

                return ApiResponse<DeviceStatisticsDto>.Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting device statistics");
                return ApiResponse<DeviceStatisticsDto>.Fail(ex.Message);
            }
        }
    }

    public class GetVerificationStatisticsQueryHandler : IRequestHandler<GetVerificationStatisticsQuery, ApiResponse<VerificationStatisticsDto>>
    {
        private readonly IMongoCollection<IdentityVerificationReadModel> _collection;
        private readonly ILogger<GetVerificationStatisticsQueryHandler> _logger;

        public GetVerificationStatisticsQueryHandler(IMongoDatabase database, ILogger<GetVerificationStatisticsQueryHandler> logger)
        {
            _collection = database.GetCollection<IdentityVerificationReadModel>("identity_verifications_read");
            _logger = logger;
        }

        public async Task<ApiResponse<VerificationStatisticsDto>> Handle(GetVerificationStatisticsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var verifications = await _collection.Find(_ => true).ToListAsync(cancellationToken);

                var dailyTrend = verifications
                    .Where(v => v.InitiatedAt >= DateTime.UtcNow.AddDays(-30))
                    .GroupBy(v => v.InitiatedAt.Date)
                    .Select(g => new DailyVerificationSummaryDto
                    {
                        Date = g.Key,
                        Total = g.Count(),
                        Approved = g.Count(v => v.VerificationStatus == "Approved"),
                        Rejected = g.Count(v => v.VerificationStatus == "Rejected")
                    })
                    .OrderBy(d => d.Date)
                    .ToList();

                var statistics = new VerificationStatisticsDto
                {
                    TotalVerifications = verifications.Count,
                    Approved = verifications.Count(v => v.VerificationStatus == "Approved"),
                    Rejected = verifications.Count(v => v.VerificationStatus == "Rejected"),
                    Pending = verifications.Count(v => v.VerificationStatus == "Pending"),
                    Expired = verifications.Count(v => v.VerificationStatus == "Expired"),
                    AverageScore = 0,
                    VerificationsByDocumentType = verifications.GroupBy(v => v.DocumentType).ToDictionary(g => g.Key, g => g.Count()),
                    VerificationsByStatus = verifications.GroupBy(v => v.VerificationStatus).ToDictionary(g => g.Key, g => g.Count()),
                    DailyTrend = dailyTrend,
                    LivenessPassRate = verifications.Any(v => v.LivenessPassed.HasValue)
                        ? (decimal)verifications.Count(v => v.LivenessPassed == true) / verifications.Count(v => v.LivenessPassed.HasValue) * 100
                        : 0,
                    BiometricRegistrations = verifications.Count(v => v.BiometricRegistered)
                };

                return ApiResponse<VerificationStatisticsDto>.Ok(statistics);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting verification statistics");
                return ApiResponse<VerificationStatisticsDto>.Fail(ex.Message);
            }
        }
    }
}
