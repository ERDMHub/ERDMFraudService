using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;

namespace ERDM.Fraud.Service.Infrastructure.Services
{
    public interface IFraudAlertService
    {
        // Command Operations
        Task<ApiResponse<FraudAlertResponseDto>> CreateAsync(CreateFraudAlertCommand command);
        Task<ApiResponse<bool>> ResolveAsync(string alertId);
        Task<ApiResponse<bool>> DeleteAsync(string alertId);

        // Query Operations
        Task<ApiResponse<FraudAlertResponseDto>> GetByIdAsync(string alertId);
        Task<ApiResponse<List<FraudAlertResponseDto>>> GetByCustomerIdAsync(string customerId);
        Task<ApiResponse<List<FraudAlertResponseDto>>> GetUnresolvedAsync(string? severity = null);
        Task<ApiResponse<List<FraudAlertResponseDto>>> GetAllAsync();
    }
}
