using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;
using ERDMCore.Infrastructure.MongoDB.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Write
{
    public class DeviceFingerprintWriteRepository : MongoRepository<DeviceFingerprint>, IDeviceFingerprintWriteRepository
    {
        public DeviceFingerprintWriteRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings, ILogger<DeviceFingerprintWriteRepository> logger)
            : base(database, settings, logger)
        {
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var indexModels = new List<CreateIndexModel<DeviceFingerprint>>
            {
                new CreateIndexModel<DeviceFingerprint>(
                    Builders<DeviceFingerprint>.IndexKeys.Ascending(x => x.DeviceId),
                    new CreateIndexOptions { Unique = true, Name = "idx_device_id" }),
                new CreateIndexModel<DeviceFingerprint>(
                    Builders<DeviceFingerprint>.IndexKeys.Ascending(x => x.FingerprintHash),
                    new CreateIndexOptions { Unique = true, Name = "idx_fingerprint_hash" }),
                new CreateIndexModel<DeviceFingerprint>(
                    Builders<DeviceFingerprint>.IndexKeys.Ascending(x => x.CustomerId),
                    new CreateIndexOptions { Name = "idx_customer_id" })
            };

            foreach (var index in indexModels)
            {
                try { _collection.Indexes.CreateOne(index); }
                catch (Exception ex) { _logger.LogWarning(ex, "Error creating index"); }
            }
        }

        public async Task<DeviceFingerprint?> GetByFingerprintHashAsync(string fingerprintHash, CancellationToken cancellationToken = default)
        {
            var filter = Builders<DeviceFingerprint>.Filter.Eq(x => x.FingerprintHash, fingerprintHash);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<DeviceFingerprint?> GetByDeviceIdAsync(string deviceId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<DeviceFingerprint>.Filter.Eq(x => x.DeviceId, deviceId);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<DeviceFingerprint>> GetBlacklistedDevicesAsync(CancellationToken cancellationToken = default)
        {
            var filter = Builders<DeviceFingerprint>.Filter.Eq(x => x.IsBlacklisted, true);
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task UpdateRiskScoreAsync(string deviceId, int newScore, CancellationToken cancellationToken = default)
        {
            var filter = Builders<DeviceFingerprint>.Filter.Eq(x => x.DeviceId, deviceId);
            var update = Builders<DeviceFingerprint>.Update
                .Set(x => x.RiskScore, newScore)
                .Set(x => x.RiskLevel, CalculateRiskLevel(newScore))
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }

        private static RiskLevel CalculateRiskLevel(int score) => score switch
        {
            >= 80 => RiskLevel.VeryHigh,
            >= 60 => RiskLevel.High,
            >= 40 => RiskLevel.Medium,
            >= 20 => RiskLevel.Low,
            _ => RiskLevel.VeryLow
        };
    }
}
