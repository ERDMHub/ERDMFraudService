using ERDM.Fraud.Service.Infrastructure.ReadModels;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Read
{
    public class FraudCaseReadRepository : IFraudCaseReadRepository
    {
        private readonly IMongoCollection<FraudCaseReadModel> _collection;

        public FraudCaseReadRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<FraudCaseReadModel>("fraud_cases_read");
        }

        public async Task<FraudCaseReadModel?> GetByCaseIdAsync(string caseId)
        {
            var filter = Builders<FraudCaseReadModel>.Filter.Eq(x => x.CaseId, caseId);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<FraudCaseReadModel>> GetByCustomerIdAsync(string customerId)
        {
            var filter = Builders<FraudCaseReadModel>.Filter.Eq(x => x.CustomerId, customerId);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<FraudCaseReadModel>> GetByStatusAsync(string status)
        {
            var filter = Builders<FraudCaseReadModel>.Filter.Eq(x => x.Status, status);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<FraudCaseReadModel>> GetByFraudTypeAsync(string fraudType)
        {
            var filter = Builders<FraudCaseReadModel>.Filter.Eq(x => x.FraudType, fraudType);
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<FraudCaseReadModel>> GetHighRiskCasesAsync(int minRiskScore)
        {
            var filter = Builders<FraudCaseReadModel>.Filter.Gte(x => x.RiskScore, minRiskScore);
            return await _collection.Find(filter).ToListAsync();
        }
    }
}
