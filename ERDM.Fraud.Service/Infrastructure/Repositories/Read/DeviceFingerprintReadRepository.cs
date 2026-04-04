using ERDM.Fraud.Service.Infrastructure.ReadModels;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Read
{
    public class DeviceFingerprintReadRepository : IDeviceFingerprintReadRepository
    {
        private readonly IMongoCollection<DeviceFingerprintReadModel> _collection;

        public DeviceFingerprintReadRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<DeviceFingerprintReadModel>("device_fingerprints_read");
        }

        public async Task<DeviceFingerprintReadModel?> GetByDeviceIdAsync(string deviceId)
        {
            var filter = Builders<DeviceFingerprintReadModel>.Filter.Eq(x => x.DeviceId, deviceId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<DeviceFingerprintReadModel?> GetByFingerprintHashAsync(string fingerprintHash)
        {
            var filter = Builders<DeviceFingerprintReadModel>.Filter.Eq(x => x.FingerprintHash, fingerprintHash);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<DeviceFingerprintReadModel>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<DeviceFingerprintReadModel>.Filter.Eq(x => x.CustomerId, customerId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<DeviceFingerprintReadModel>> GetBlacklistedAsync(int? page, int? pageSize)
        {
            var filter = Builders<DeviceFingerprintReadModel>.Filter.Eq(x => x.IsBlacklisted, true);
            var query = _collection.Find(filter);

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Limit(pageSize.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<DeviceFingerprintReadModel>> GetHighRiskAsync(int minRiskScore, int limit)
        {
            var filter = Builders<DeviceFingerprintReadModel>.Filter.Gte(x => x.RiskScore, minRiskScore);
            return await _collection.Find(filter).Limit(limit).ToListAsync();
        }
    }
}
