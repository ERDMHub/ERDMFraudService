using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Service.Application.Queries
{
    public class GetFraudDashboardSummaryQuery : IRequest<ApiResponse<FraudDashboardSummaryDto>>
    {
    }

    public class GetFraudTrendsQuery : IRequest<ApiResponse<FraudTrendsDto>>
    {
        public int Months { get; set; } = 6;
    }

    public class GetTopFraudTypesQuery : IRequest<ApiResponse<List<TopFraudTypeDto>>>
    {
        public int Limit { get; set; } = 10;
    }

    public class GetRiskDistributionQuery : IRequest<ApiResponse<RiskDistributionDto>>
    {
    }

    public class GetDeviceStatisticsQuery : IRequest<ApiResponse<DeviceStatisticsDto>>
    {
    }

    public class GetVerificationStatisticsQuery : IRequest<ApiResponse<VerificationStatisticsDto>>
    {
    }

    // Additional missing queries from controllers
    public class GetIdentityVerificationByIdQuery : IRequest<ApiResponse<IdentityVerificationResponseDto>>
    {
        public string VerificationId { get; set; } = string.Empty;
    }

    public class GetIdentityVerificationsByCustomerQuery : IRequest<ApiResponse<List<IdentityVerificationResponseDto>>>
    {
        public string CustomerId { get; set; } = string.Empty;
    }

    public class GetPendingVerificationsQuery : IRequest<ApiResponse<PaginatedResponse<IdentityVerificationResponseDto>>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetFraudCasesByTypeQuery : IRequest<ApiResponse<PaginatedResponse<FraudCaseResponseDto>>>
    {
        public string FraudType { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetHighRiskCasesQuery : IRequest<ApiResponse<List<FraudCaseResponseDto>>>
    {
        public int MinRiskScore { get; set; } = 60;
    }

    public class GetAvailableSanctionsListsQuery : IRequest<ApiResponse<List<SanctionsListDto>>>
    {
    }

    public class GetSyntheticIdentityIndicatorsQuery : IRequest<ApiResponse<List<SyntheticIdentityIndicatorDto>>>
    {
    }

    public class GetCustomerSyntheticRiskQuery : IRequest<ApiResponse<CustomerSyntheticRiskDto>>
    {
        public string CustomerId { get; set; } = string.Empty;
    }
}
