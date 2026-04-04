using AutoMapper;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;
using ERDM.Fraud.Service.Domain.ValueObjects;
using ERDM.Fraud.Service.Infrastructure.Repositories.Write;
using MediatR;

namespace ERDM.Fraud.Service.Application.Commands.Handlers
{
    public class OpenFraudCaseCommandHandler : IRequestHandler<OpenFraudCaseCommand, ApiResponse<FraudCaseResponseDto>>
    {
        private readonly IFraudCaseWriteRepository _writeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OpenFraudCaseCommandHandler> _logger;

        public OpenFraudCaseCommandHandler(
            IFraudCaseWriteRepository writeRepository,
            IMapper mapper,
            ILogger<OpenFraudCaseCommandHandler> logger)
        {
            _writeRepository = writeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<FraudCaseResponseDto>> Handle(OpenFraudCaseCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Opening fraud case for customer {CustomerId}", command.CustomerId);

                var fraudCase = FraudCase.Create(
                    command.CustomerId,
                    Enum.Parse<FraudType>(command.FraudType),
                    command.RiskScore
                );

                if (!string.IsNullOrEmpty(command.Description))
                {
                    fraudCase.AddEvidence(new FraudEvidence
                    {
                        EvidenceType = "InitialReport",
                        Description = command.Description,
                        CollectedAt = DateTime.UtcNow,
                        CollectedBy = command.UserId ?? "system"
                    });
                }

                await _writeRepository.AddAsync(fraudCase, cancellationToken);

                var response = _mapper.Map<FraudCaseResponseDto>(fraudCase);
                return ApiResponse<FraudCaseResponseDto>.Ok(response, "Fraud case opened");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error opening fraud case for customer {CustomerId}", command.CustomerId);
                return ApiResponse<FraudCaseResponseDto>.Fail(ex.Message);
            }
        }
    }
}
