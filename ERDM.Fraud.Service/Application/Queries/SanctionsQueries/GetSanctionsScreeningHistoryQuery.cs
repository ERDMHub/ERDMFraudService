using ERDM.Fraud.Service.Application.Common;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;

namespace ERDM.Fraud.Service.Application.Queries
{
    public class GetSanctionsScreeningHistoryQuery : QueryBase<PaginatedResponse<SanctionsScreeningResponseDto>>
    {
        public string CustomerId { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetSanctionsHitsQuery : QueryBase<List<SanctionHitDto>>
    {
        public string? CustomerId { get; set; }
        public string? ListName { get; set; }
        public DateTime? FromDate { get; set; }
    }
}
