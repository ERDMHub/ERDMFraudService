using ERDM.Fraud.Service.Application.Common;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;

namespace ERDM.Fraud.Service.Application.Queries
{
    public class GetDeviceByIdQuery : QueryBase<DeviceFingerprintResponseDto>
    {
        public string DeviceId { get; set; } = string.Empty;
    }

    public class GetDeviceByFingerprintQuery : QueryBase<DeviceFingerprintResponseDto>
    {
        public string FingerprintHash { get; set; } = string.Empty;
    }

    public class GetDevicesByCustomerQuery : QueryBase<List<DeviceFingerprintResponseDto>>
    {
        public string CustomerId { get; set; } = string.Empty;
    }

    public class GetBlacklistedDevicesQuery : QueryBase<PaginatedResponse<DeviceFingerprintResponseDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    public class GetHighRiskDevicesQuery : QueryBase<List<DeviceFingerprintResponseDto>>
    {
        public int MinRiskScore { get; set; } = 60;
        public int Take { get; set; } = 100;
    }
}
