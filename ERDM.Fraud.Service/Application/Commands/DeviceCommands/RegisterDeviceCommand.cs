using ERDM.Fraud.Application.Common;
using ERDM.Fraud.Service.Contracts.Dtos;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class RegisterDeviceCommand : CommandBase<DeviceFingerprintResponseDto>
    {
        public string FingerprintHash { get; set; } = string.Empty;
        public string DeviceType { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public BrowserInfoDto BrowserInfo { get; set; } = new();
        public HardwareInfoDto HardwareInfo { get; set; } = new();
        public NetworkInfoDto NetworkInfo { get; set; } = new();
    }
}
