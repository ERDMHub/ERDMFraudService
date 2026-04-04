using ERDM.Fraud.Service.Infrastructure.ReadModels;

namespace ERDM.Fraud.Service.Infrastructure.Repositories.Read
{
    public interface IFraudCaseReadRepository
    {
        Task<FraudCaseReadModel?> GetByCaseIdAsync(string caseId);
        Task<List<FraudCaseReadModel>> GetByCustomerIdAsync(string customerId);
        Task<List<FraudCaseReadModel>> GetByStatusAsync(string status);
        Task<List<FraudCaseReadModel>> GetByFraudTypeAsync(string fraudType);
        Task<List<FraudCaseReadModel>> GetHighRiskCasesAsync(int minRiskScore);
    }
}
