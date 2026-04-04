using ERDM.Fraud.Application.Common;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class RegisterBiometricCommand : CommandBase<bool>
    {
        public string CustomerId { get; set; } = string.Empty;
        public string BiometricData { get; set; } = string.Empty;
        public string BiometricType { get; set; } = string.Empty;
    }
}
