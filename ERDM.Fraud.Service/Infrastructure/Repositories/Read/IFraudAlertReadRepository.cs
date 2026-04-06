using ERDM.Fraud.Service.Infrastructure.ReadModels;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Read
{
    public interface IFraudAlertReadRepository
    {
        Task<FraudAlertReadModel?> GetByAlertIdAsync(string alertId, CancellationToken cancellationToken = default);
        Task<List<FraudAlertReadModel>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
        Task<List<FraudAlertReadModel>> GetUnresolvedAsync(string? severity = null, CancellationToken cancellationToken = default);
        Task<List<FraudAlertReadModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<long> GetCountAsync(CancellationToken cancellationToken = default);
    }
}
