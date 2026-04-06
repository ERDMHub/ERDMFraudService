using AutoMapper;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Infrastructure.Repositories.Write;
using MediatR;


namespace ERDM.Fraud.Service.Application.Commands.Handlers
{

    // Handler for CreateFraudAlertCommand - Using AutoMapper
    public class CreateFraudAlertCommandHandler : IRequestHandler<CreateFraudAlertCommand, ApiResponse<FraudAlertResponseDto>>
    {
        private readonly IFraudAlertWriteRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateFraudAlertCommandHandler> _logger;

        public CreateFraudAlertCommandHandler(
            IFraudAlertWriteRepository repository,
            IMapper mapper,
            ILogger<CreateFraudAlertCommandHandler> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<FraudAlertResponseDto>> Handle(CreateFraudAlertCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating fraud alert for customer {CustomerId}", command.CustomerId);

                // Create entity using factory method
                var alert = FraudAlert.Create(
                    command.CustomerId,
                    command.AlertType,
                    command.Severity,
                    command.Description
                );

                // Save to database
                await _repository.AddAsync(alert, cancellationToken);

                // Map to response DTO using AutoMapper
                var response = _mapper.Map<FraudAlertResponseDto>(alert);

                return ApiResponse<FraudAlertResponseDto>.Ok(response, "Alert created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating alert for customer {CustomerId}", command.CustomerId);
                return ApiResponse<FraudAlertResponseDto>.Fail(ex.Message);
            }
        }
    }

    // Handler for ResolveFraudAlertCommand
    public class ResolveFraudAlertCommandHandler : IRequestHandler<ResolveFraudAlertCommand, ApiResponse<bool>>
    {
        private readonly IFraudAlertWriteRepository _repository;
        private readonly ILogger<ResolveFraudAlertCommandHandler> _logger;

        public ResolveFraudAlertCommandHandler(IFraudAlertWriteRepository repository, ILogger<ResolveFraudAlertCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ApiResponse<bool>> Handle(ResolveFraudAlertCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Resolving fraud alert {AlertId}", command.AlertId);

                var alert = await _repository.GetByAlertIdAsync(command.AlertId, cancellationToken);
                if (alert == null)
                    return ApiResponse<bool>.Fail($"Alert {command.AlertId} not found");

                alert.Resolve();
                await _repository.UpdateAsync(alert, cancellationToken);

                return ApiResponse<bool>.Ok(true, "Alert resolved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resolving alert {AlertId}", command.AlertId);
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }

    // Handler for DeleteFraudAlertCommand
    public class DeleteFraudAlertCommandHandler : IRequestHandler<DeleteFraudAlertCommand, ApiResponse<bool>>
    {
        private readonly IFraudAlertWriteRepository _repository;
        private readonly ILogger<DeleteFraudAlertCommandHandler> _logger;

        public DeleteFraudAlertCommandHandler(IFraudAlertWriteRepository repository, ILogger<DeleteFraudAlertCommandHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ApiResponse<bool>> Handle(DeleteFraudAlertCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(command.AlertId))
                    return ApiResponse<bool>.Fail("AlertId cannot be null or empty");

                _logger.LogInformation("Deleting fraud alert {AlertId}", command.AlertId);

                var alert = await _repository.GetByAlertIdAsync(command.AlertId, cancellationToken);

                await _repository.DeleteAsync(alert, cancellationToken);

                return ApiResponse<bool>.Ok(true, "Alert deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting alert {AlertId}", command.AlertId);
                return ApiResponse<bool>.Fail(ex.Message);
            }
        }
    }

}

