using ERDM.Fraud.Application.Common;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class BlacklistDeviceCommand : CommandBase<bool>
    {
        public string DeviceId { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string BlacklistedBy { get; set; } = string.Empty;
    }
}
