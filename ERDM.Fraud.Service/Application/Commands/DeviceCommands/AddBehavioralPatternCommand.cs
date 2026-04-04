using ERDM.Fraud.Application.Common;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class AddBehavioralPatternCommand : CommandBase<bool>
    {
        public string DeviceId { get; set; } = string.Empty;
        public string PatternType { get; set; } = string.Empty;
        public string PatternValue { get; set; } = string.Empty;
        public decimal Confidence { get; set; }
    }
}
