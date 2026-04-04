using ERDM.Core.Interfaces;
using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;
using ERDMCore.Infrastructure.MongoDB.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Write
{
    public interface IIdentityVerificationWriteRepository : IRepository<IdentityVerification>
    {
        Task<IdentityVerification?> GetByVerificationIdAsync(string verificationId, CancellationToken cancellationToken = default);
        Task<IdentityVerification?> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
        Task<List<IdentityVerification>> GetPendingVerificationsAsync(CancellationToken cancellationToken = default);
        Task<List<IdentityVerification>> GetExpiredVerificationsAsync(CancellationToken cancellationToken = default);
        Task UpdateVerificationStatusAsync(string verificationId, VerificationStatus status, CancellationToken cancellationToken = default);
    }

    
}
