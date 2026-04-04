using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;
using ERDMCore.Infrastructure.MongoDB.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Write
{
    public class IdentityVerificationWriteRepository : MongoRepository<IdentityVerification>, IIdentityVerificationWriteRepository
    {
        public IdentityVerificationWriteRepository(
            IMongoDatabase database,
            IOptions<MongoDbSettings> settings,
            ILogger<IdentityVerificationWriteRepository> logger)
            : base(database, settings, logger)
        {
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var indexModels = new List<CreateIndexModel<IdentityVerification>>
            {
                new CreateIndexModel<IdentityVerification>(
                    Builders<IdentityVerification>.IndexKeys.Ascending(x => x.VerificationId),
                    new CreateIndexOptions { Unique = true, Name = "idx_verification_id" }),
                new CreateIndexModel<IdentityVerification>(
                    Builders<IdentityVerification>.IndexKeys.Ascending(x => x.CustomerId),
                    new CreateIndexOptions { Name = "idx_customer_id" }),
                new CreateIndexModel<IdentityVerification>(
                    Builders<IdentityVerification>.IndexKeys.Combine(
                        Builders<IdentityVerification>.IndexKeys.Ascending(x => x.VerificationStatus),
                        Builders<IdentityVerification>.IndexKeys.Ascending(x => x.ExpiresAt)),
                    new CreateIndexOptions { Name = "idx_status_expiry" })
            };

            foreach (var index in indexModels)
            {
                try { _collection.Indexes.CreateOne(index); }
                catch (Exception ex) { _logger.LogWarning(ex, "Error creating index"); }
            }
        }

        public async Task<IdentityVerification?> GetByVerificationIdAsync(string verificationId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<IdentityVerification>.Filter.Eq(x => x.VerificationId, verificationId);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IdentityVerification?> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<IdentityVerification>.Filter.Eq(x => x.CustomerId, customerId);
            var sort = Builders<IdentityVerification>.Sort.Descending(x => x.CreatedAt);
            return await _collection.Find(filter).Sort(sort).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<IdentityVerification>> GetPendingVerificationsAsync(CancellationToken cancellationToken = default)
        {
            var filter = Builders<IdentityVerification>.Filter.Eq(x => x.VerificationStatus, VerificationStatus.Pending);
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task<List<IdentityVerification>> GetExpiredVerificationsAsync(CancellationToken cancellationToken = default)
        {
            var filter = Builders<IdentityVerification>.Filter.And(
                Builders<IdentityVerification>.Filter.Eq(x => x.VerificationStatus, VerificationStatus.Pending),
                Builders<IdentityVerification>.Filter.Lt(x => x.ExpiresAt, DateTime.UtcNow)
            );
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task UpdateVerificationStatusAsync(string verificationId, VerificationStatus status, CancellationToken cancellationToken = default)
        {
            var filter = Builders<IdentityVerification>.Filter.Eq(x => x.VerificationId, verificationId);
            var update = Builders<IdentityVerification>.Update
                .Set(x => x.VerificationStatus, status)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);
            await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}
