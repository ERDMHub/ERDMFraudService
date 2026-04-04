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
    public class RegisterDeviceCommandHandler : IRequestHandler<RegisterDeviceCommand, ApiResponse<DeviceFingerprintResponseDto>>
    {
        private readonly IDeviceFingerprintWriteRepository _writeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterDeviceCommandHandler> _logger;
        private readonly IMediator _mediator;

        public RegisterDeviceCommandHandler(
            IDeviceFingerprintWriteRepository writeRepository,
            IMapper mapper,
            ILogger<RegisterDeviceCommandHandler> logger,
            IMediator mediator)
        {
            _writeRepository = writeRepository;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ApiResponse<DeviceFingerprintResponseDto>> Handle(RegisterDeviceCommand command, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Registering device with fingerprint {Fingerprint}", command.FingerprintHash);

                // Check if device already exists
                var existingDevice = await _writeRepository.GetByFingerprintHashAsync(command.FingerprintHash);
                if (existingDevice != null)
                {
                    return ApiResponse<DeviceFingerprintResponseDto>.Ok(
                        _mapper.Map<DeviceFingerprintResponseDto>(existingDevice),
                        "Device already registered");
                }

                // Create new device
                var device = DeviceFingerprint.Create(
                    command.FingerprintHash,
                    Enum.Parse<DeviceType>(command.DeviceType),
                    command.OperatingSystem,
                    _mapper.Map<BrowserInfo>(command.BrowserInfo),
                    _mapper.Map<HardwareInfo>(command.HardwareInfo),
                    _mapper.Map<NetworkInfo>(command.NetworkInfo)
                );

                // Calculate initial risk score based on device characteristics
                var riskScore = CalculateInitialRiskScore(device);
                device.UpdateRiskScore(riskScore);

                await _writeRepository.AddAsync(device, cancellationToken);

                var response = _mapper.Map<DeviceFingerprintResponseDto>(device);
                return ApiResponse<DeviceFingerprintResponseDto>.Ok(response, "Device registered successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering device");
                return ApiResponse<DeviceFingerprintResponseDto>.Fail(ex.Message);
            }
        }

        private int CalculateInitialRiskScore(DeviceFingerprint device)
        {
            int score = 0;

            // Check for VPN/Proxy
            if (device.NetworkInfo.IsVpn || device.NetworkInfo.IsProxy)
                score += 30;

            // Check for Tor
            if (device.NetworkInfo.IsTor)
                score += 50;

            // Check for emulator
            if (device.HardwareInfo.IsEmulator)
                score += 40;

            // Check for rooted device
            if (device.HardwareInfo.IsRooted)
                score += 35;

            // Check for suspicious browser settings
            if (!device.BrowserInfo.JavascriptEnabled)
                score += 20;

            return Math.Min(score, 100);
        }
    }
}
