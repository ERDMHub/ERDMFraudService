using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.ValueObjects;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class FraudCaseResolvedEvent : FraudDomainEventBase
    {
        public FraudCaseResolvedEvent(FraudCase fraudCase, FraudResolution resolution)
        {
            EntityId = fraudCase.Id;
            EntityType = nameof(FraudCase);
            CaseId = fraudCase.CaseId;
            ResolutionType = resolution.ResolutionType;
            ActionTaken = resolution.ActionTaken;
            ResolvedAt = resolution.ResolvedAt;
            ResolvedBy = resolution.ResolvedBy;
        }

        public string CaseId { get; }
        public string ResolutionType { get; }
        public string? ActionTaken { get; }
        public DateTime ResolvedAt { get; }
        public string ResolvedBy { get; }
    }
}
