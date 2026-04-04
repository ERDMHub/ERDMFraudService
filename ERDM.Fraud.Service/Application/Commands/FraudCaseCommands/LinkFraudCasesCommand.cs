using ERDM.Fraud.Application.Common;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class LinkFraudCasesCommand : CommandBase<bool>
    {
        public string PrimaryCaseId { get; set; } = string.Empty;
        public string SecondaryCaseId { get; set; } = string.Empty;
    }
}
