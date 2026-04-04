using ERDM.Fraud.Application.Common;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class AssociateDeviceWithCustomerCommand : CommandBase<bool>
    {
        public string DeviceId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
    }

}
