using ERDM.Fraud.Service.Domain.Entities;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class FraudCaseLinkedEvent : FraudDomainEventBase
    {
        public FraudCaseLinkedEvent(FraudCase fraudCase, string linkedCaseId)
        {
            EntityId = fraudCase.Id;
            EntityType = nameof(FraudCase);
            CaseId = fraudCase.CaseId;
            LinkedCaseId = linkedCaseId;
            LinkedAt = DateTime.UtcNow;
        }

        public string CaseId { get; }
        public string LinkedCaseId { get; }
        public DateTime LinkedAt { get; }
    }
}
