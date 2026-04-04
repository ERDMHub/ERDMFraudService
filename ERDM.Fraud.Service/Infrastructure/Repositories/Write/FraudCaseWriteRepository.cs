using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;
using ERDMCore.Infrastructure.MongoDB.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Write
{
    public class FraudCaseWriteRepository : MongoRepository<FraudCase>, IFraudCaseWriteRepository
    {
        public FraudCaseWriteRepository(
            IMongoDatabase database,
            IOptions<MongoDbSettings> settings,
            ILogger<FraudCaseWriteRepository> logger)
            : base(database, settings, logger)
        {
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var indexModels = new List<CreateIndexModel<FraudCase>>
            {
                new CreateIndexModel<FraudCase>(
                    Builders<FraudCase>.IndexKeys.Ascending(x => x.CaseId),
                    new CreateIndexOptions { Unique = true, Name = "idx_case_id" }),
                new CreateIndexModel<FraudCase>(
                    Builders<FraudCase>.IndexKeys.Ascending(x => x.CustomerId),
                    new CreateIndexOptions { Name = "idx_customer_id" }),
                new CreateIndexModel<FraudCase>(
                    Builders<FraudCase>.IndexKeys.Ascending(x => x.Status),
                    new CreateIndexOptions { Name = "idx_status" }),
                new CreateIndexModel<FraudCase>(
                    Builders<FraudCase>.IndexKeys.Ascending(x => x.RiskScore),
                    new CreateIndexOptions { Name = "idx_risk_score" })
            };

            foreach (var index in indexModels)
            {
                try { _collection.Indexes.CreateOne(index); }
                catch (Exception ex) { _logger.LogWarning(ex, "Error creating index"); }
            }
        }

        public async Task<FraudCase?> GetByCaseIdAsync(string caseId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<FraudCase>.Filter.Eq(x => x.CaseId, caseId);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<FraudCase>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<FraudCase>.Filter.Eq(x => x.CustomerId, customerId);
            var sort = Builders<FraudCase>.Sort.Descending(x => x.CreatedAt);
            return await _collection.Find(filter).Sort(sort).ToListAsync(cancellationToken);
        }

        public async Task<List<FraudCase>> GetByStatusAsync(FraudCaseStatus status, CancellationToken cancellationToken = default)
        {
            var filter = Builders<FraudCase>.Filter.Eq(x => x.Status, status);
            var sort = Builders<FraudCase>.Sort.Descending(x => x.CreatedAt);
            return await _collection.Find(filter).Sort(sort).ToListAsync(cancellationToken);
        }

        public async Task<List<FraudCase>> GetLinkedCasesAsync(string caseId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<FraudCase>.Filter.AnyEq(x => x.LinkedCases, caseId);
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }

        public async Task UpdateCaseStatusAsync(string caseId, FraudCaseStatus status, string updatedBy, CancellationToken cancellationToken = default)
        {
            var filter = Builders<FraudCase>.Filter.Eq(x => x.CaseId, caseId);
            var update = Builders<FraudCase>.Update
                .Set(x => x.Status, status)
                .Set(x => x.UpdatedAt, DateTime.UtcNow)
                .Set(x => x.UpdatedBy, updatedBy);
            await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }

        public async Task AssignCaseAsync(string caseId, string assignedTo, CancellationToken cancellationToken = default)
        {
            var filter = Builders<FraudCase>.Filter.Eq(x => x.CaseId, caseId);
            var update = Builders<FraudCase>.Update
                .Set(x => x.AssignedTo, assignedTo)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);
            await _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}
