using AutoMapper;
using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Application.Queries;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using MediatR;

namespace ERDM.Fraud.Service.Infrastructure.Services
{
    public class FraudAlertService : IFraudAlertService
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<FraudAlertService> _logger;

        public FraudAlertService(IMediator mediator, IMapper mapper, ILogger<FraudAlertService> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        // Command Operations
        public async Task<ApiResponse<FraudAlertResponseDto>> CreateAsync(CreateFraudAlertCommand command)
        {
            try
            {
                _logger.LogInformation("Creating fraud alert for customer {CustomerId}", command.CustomerId);
                return await _mediator.Send(command);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating fraud alert");
                return ApiResponse<FraudAlertResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> ResolveAsync(string alertId)
        {
            try
            {
                _logger.LogInformation("Resolving fraud alert {AlertId}", alertId);
                return await _mediator.Send(new ResolveFraudAlertCommand { AlertId = alertId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resolving fraud alert {AlertId}", alertId);
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string alertId)
        {
            try
            {
                _logger.LogInformation("Deleting fraud alert {AlertId}", alertId);
                return await _mediator.Send(new DeleteFraudAlertCommand { AlertId = alertId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting fraud alert {AlertId}", alertId);
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }

        // Query Operations
        public async Task<ApiResponse<FraudAlertResponseDto>> GetByIdAsync(string alertId)
        {
            try
            {
                _logger.LogInformation("Getting fraud alert by ID {AlertId}", alertId);
                return await _mediator.Send(new GetFraudAlertByIdQuery { AlertId = alertId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting fraud alert by ID {AlertId}", alertId);
                return ApiResponse<FraudAlertResponseDto>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<FraudAlertResponseDto>>> GetByCustomerIdAsync(string customerId)
        {
            try
            {
                _logger.LogInformation("Getting fraud alerts for customer {CustomerId}", customerId);
                return await _mediator.Send(new GetFraudAlertsByCustomerQuery { CustomerId = customerId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting fraud alerts for customer {CustomerId}", customerId);
                return ApiResponse<List<FraudAlertResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<FraudAlertResponseDto>>> GetUnresolvedAsync(string? severity = null)
        {
            try
            {
                _logger.LogInformation("Getting unresolved fraud alerts with severity {Severity}", severity ?? "All");
                return await _mediator.Send(new GetUnresolvedAlertsQuery { Severity = severity });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting unresolved fraud alerts");
                return ApiResponse<List<FraudAlertResponseDto>>.Fail(ex.Message);
            }
        }

        public async Task<ApiResponse<List<FraudAlertResponseDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Getting all fraud alerts");
                return await _mediator.Send(new GetAllFraudAlertsQuery());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all fraud alerts");
                return ApiResponse<List<FraudAlertResponseDto>>.Fail(ex.Message);
            }
        }
    }
}
