using AutoMapper;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;
using ERDM.Fraud.Service.Infrastructure.Repositories.Write;
using ERDM.Fraud.Service.Infrastructure.Services;
using MediatR;

namespace ERDM.Fraud.Service.Application.Commands.Handlers
{
    public class InitiateIdentityVerificationCommandHandler : IRequestHandler<InitiateIdentityVerificationCommand, ApiResponse<IdentityVerificationResponseDto>>
    {
        private readonly IIdentityVerificationWriteRepository _writeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<InitiateIdentityVerificationCommandHandler> _logger;
        private readonly IIdentityVerificationService _verificationService;
        private readonly IMediator _mediator;  // Add this line

        public InitiateIdentityVerificationCommandHandler(
            IIdentityVerificationWriteRepository writeRepository,
            IMapper mapper,
            ILogger<InitiateIdentityVerificationCommandHandler> logger,
            IIdentityVerificationService verificationService,
            IMediator mediator)  // Add mediator parameter
        {
            _writeRepository = writeRepository;
            _mapper = mapper;
            _logger = logger;
            _verificationService = verificationService;
            _mediator = mediator;  // Initialize mediator
        }

        public async Task<ApiResponse<IdentityVerificationResponseDto>> Handle(InitiateIdentityVerificationCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Initiating identity verification for customer {CustomerId}", command.CustomerId);

                // Create verification record
                var verification = IdentityVerification.Create(
                    command.CustomerId,
                    Enum.Parse<DocumentType>(command.DocumentType),
                    command.DocumentNumber
                );

                await _writeRepository.AddAsync(verification, cancellationToken);

                // Start verification process (async)
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var result = await _verificationService.VerifyDocument(
                            command.DocumentImageUrl,
                            command.SelfieImageUrl,
                            command.DocumentType
                        );

                        var completeCommand = new CompleteIdentityVerificationCommand
                        {
                            VerificationId = verification.VerificationId,
                            VerificationScore = result.Score,
                            Details = result.Details,
                            VerifiedBy = "system"
                        };

                        // Now _mediator is available
                        await _mediator.Send(completeCommand, cancellationToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in async verification process for customer {CustomerId}", command.CustomerId);
                    }
                });

                var response = _mapper.Map<IdentityVerificationResponseDto>(verification);
                return ApiResponse<IdentityVerificationResponseDto>.Ok(response, "Verification initiated");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating verification for customer {CustomerId}", command.CustomerId);
                return ApiResponse<IdentityVerificationResponseDto>.Fail(ex.Message);
            }
        }
    }
}