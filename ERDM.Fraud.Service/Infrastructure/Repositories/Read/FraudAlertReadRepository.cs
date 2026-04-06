using ERDM.Fraud.Service.Infrastructure.ReadModels;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Read
{
    public class FraudAlertReadRepository : IFraudAlertReadRepository
    {
        private readonly IMongoCollection<FraudAlertReadModel> _collection;
        private readonly ILogger<FraudAlertReadRepository> _logger;

        public FraudAlertReadRepository(IMongoDatabase database, ILogger<FraudAlertReadRepository> logger)
        {
            _collection = database.GetCollection<FraudAlertReadModel>("fraud_alerts_read");
            _logger = logger;
        }

        public async Task<FraudAlertReadModel?> GetByAlertIdAsync(string alertId, CancellationToken cancellationToken = default)
        {
            try
            {
                var filter = Builders<FraudAlertReadModel>.Filter.Eq(x => x.AlertId, alertId);
                return await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting alert by ID {AlertId}", alertId);
                throw;
            }
        }

        public async Task<List<FraudAlertReadModel>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default)
        {
            try
            {
                var filter = Builders<FraudAlertReadModel>.Filter.Eq(x => x.CustomerId, customerId);
                var sort = Builders<FraudAlertReadModel>.Sort.Descending(x => x.CreatedAt);
                return await _collection.Find(filter).Sort(sort).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting alerts for customer {CustomerId}", customerId);
                throw;
            }
        }

        public async Task<List<FraudAlertReadModel>> GetUnresolvedAsync(string? severity = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var filterBuilder = Builders<FraudAlertReadModel>.Filter;
                var filter = filterBuilder.Eq(x => x.IsResolved, false);

                if (!string.IsNullOrEmpty(severity))
                    filter = filterBuilder.And(filter, filterBuilder.Eq(x => x.Severity, severity));

                var sort = Builders<FraudAlertReadModel>.Sort.Descending(x => x.CreatedAt);
                return await _collection.Find(filter).Sort(sort).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unresolved alerts");
                throw;
            }
        }

        public async Task<List<FraudAlertReadModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var sort = Builders<FraudAlertReadModel>.Sort.Descending(x => x.CreatedAt);
                return await _collection.Find(_ => true).Sort(sort).ToListAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all alerts");
                throw;
            }
        }

        public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                return await _collection.CountDocumentsAsync(_ => true, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting alert count");
                throw;
            }
        }
    }
}
