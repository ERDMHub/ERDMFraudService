using ERDM.Core.Interfaces;
using ERDM.Fraud.Service.Domain.Entities;


namespace ERDM.Fraud.Service.Infrastructure.Repositories.Write
{
    public interface IDeviceFingerprintWriteRepository : IRepository<DeviceFingerprint>
    {
        Task<DeviceFingerprint?> GetByFingerprintHashAsync(string fingerprintHash, CancellationToken cancellationToken = default);
        Task<DeviceFingerprint?> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default);
        Task<List<DeviceFingerprint>> GetBlacklistedDevicesAsync(CancellationToken cancellationToken = default);
        Task UpdateRiskScoreAsync(string deviceId, int newScore, CancellationToken cancellationToken = default);
    }
}
