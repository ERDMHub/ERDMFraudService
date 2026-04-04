using ERDM.Fraud.Application.Common;

namespace ERDM.Fraud.Service.Application.Commands
{
    public class ResolveFraudCaseCommand : CommandBase<bool>
    {
        public string CaseId { get; set; } = string.Empty;
        public string ResolutionType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ActionTaken { get; set; }
        public string ResolvedBy { get; set; } = string.Empty;
    }
}
