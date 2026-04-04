using ERDM.Fraud.Service.Application.Common;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;

namespace ERDM.Fraud.Service.Application.Queries
{
    public class GetFraudCaseByIdQuery : QueryBase<FraudCaseResponseDto>
    {
        public string CaseId { get; set; } = string.Empty;
    }

    public class GetFraudCasesByCustomerQuery : QueryBase<List<FraudCaseResponseDto>>
    {
        public string CustomerId { get; set; } = string.Empty;
    }

    public class GetFraudCasesByStatusQuery : QueryBase<PaginatedResponse<FraudCaseResponseDto>>
    {
        public string Status { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetFraudNetworkQuery : QueryBase<FraudNetworkResponseDto>
    {
        public string CustomerId { get; set; } = string.Empty;
        public int Depth { get; set; } = 3;
    }

    public class GetFraudStatisticsQuery : QueryBase<FraudStatisticsDto>
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
