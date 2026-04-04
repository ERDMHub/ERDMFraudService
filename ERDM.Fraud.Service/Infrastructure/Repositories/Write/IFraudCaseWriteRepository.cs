using ERDM.Core.Interfaces;
using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;
using ERDMCore.Infrastructure.MongoDB.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Write
{
    public interface IFraudCaseWriteRepository : IRepository<FraudCase>
    {
        Task<FraudCase?> GetByCaseIdAsync(string caseId, CancellationToken cancellationToken = default);
        Task<List<FraudCase>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);
        Task<List<FraudCase>> GetByStatusAsync(FraudCaseStatus status, CancellationToken cancellationToken = default);
        Task<List<FraudCase>> GetLinkedCasesAsync(string caseId, CancellationToken cancellationToken = default);
        Task UpdateCaseStatusAsync(string caseId, FraudCaseStatus status, string updatedBy, CancellationToken cancellationToken = default);
        Task AssignCaseAsync(string caseId, string assignedTo, CancellationToken cancellationToken = default);
    }

    
}
