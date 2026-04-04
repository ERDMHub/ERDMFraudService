using ERDM.Fraud.Service.Infrastructure.ReadModels;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Read
{
    public interface IDeviceFingerprintReadRepository
    {
        Task<DeviceFingerprintReadModel?> GetByDeviceIdAsync(string deviceId);
        Task<DeviceFingerprintReadModel?> GetByFingerprintHashAsync(string fingerprintHash);
        Task<List<DeviceFingerprintReadModel>> GetByCustomerIdAsync(string customerId);
        Task<List<DeviceFingerprintReadModel>> GetBlacklistedAsync(int? page, int? pageSize);
        Task<List<DeviceFingerprintReadModel>> GetHighRiskAsync(int minRiskScore, int limit);
    }
}
