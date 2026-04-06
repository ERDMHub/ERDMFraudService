using ERDM.Core.Interfaces;
using ERDM.Fraud.Service.Domain.Entities;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Write
{
    public interface IFraudAlertWriteRepository : IRepository<FraudAlert>
    {
        Task<FraudAlert?> GetByAlertIdAsync(string alertId, CancellationToken cancellationToken = default);
    }
}
