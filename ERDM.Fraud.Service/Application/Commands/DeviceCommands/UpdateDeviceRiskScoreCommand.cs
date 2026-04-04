using ERDM.Fraud.Application.Common;

namespace ERDM.Fraud.Service.Application.Commands.DeviceCommands
{
    public class UpdateDeviceRiskScoreCommand : CommandBase<bool>
    {
        public string DeviceId { get; set; } = string.Empty;
        public int RiskScore { get; set; }
    }
}
