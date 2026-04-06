using ERDM.Fraud.Service.Domain.Entities;
using ERDMCore.Infrastructure.MongoDB.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Write
{
    public class FraudAlertWriteRepository : MongoRepository<FraudAlert>, IFraudAlertWriteRepository
    {
        public FraudAlertWriteRepository(IMongoDatabase database, IOptions<MongoDbSettings> settings, ILogger<FraudAlertWriteRepository> logger)
            : base(database, settings, logger)
        {
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var indexModels = new List<CreateIndexModel<FraudAlert>>
            {
                new CreateIndexModel<FraudAlert>(
                    Builders<FraudAlert>.IndexKeys.Ascending(x => x.AlertId),
                    new CreateIndexOptions { Unique = true, Name = "idx_alert_id" }),
                new CreateIndexModel<FraudAlert>(
                    Builders<FraudAlert>.IndexKeys.Ascending(x => x.CustomerId),
                    new CreateIndexOptions { Name = "idx_customer_id" })
            };

            foreach (var index in indexModels)
            {
                try { _collection.Indexes.CreateOne(index); }
                catch (Exception ex) { _logger.LogWarning(ex, "Error creating index"); }
            }
        }

        public async Task<FraudAlert?> GetByAlertIdAsync(string alertId, CancellationToken cancellationToken = default)
        {
            var filter = Builders<FraudAlert>.Filter.Eq(x => x.AlertId, alertId);
            return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
