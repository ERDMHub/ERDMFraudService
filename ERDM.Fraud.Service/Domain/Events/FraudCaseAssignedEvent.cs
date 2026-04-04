using ERDM.Core.Entities;
using ERDM.Fraud.Service.Domain.Entities;

namespace ERDM.Fraud.Service.Domain.Events
{
    // Additional events
    public class FraudCaseAssignedEvent : DomainEventBase
    {
        public FraudCaseAssignedEvent(FraudCase fraudCase, string assignee)
        {
            EntityId = fraudCase.Id;
            EntityType = nameof(FraudCase);
            CaseId = fraudCase.CaseId;
            Assignee = assignee;
            AssignedAt = DateTime.UtcNow;
        }

        public string CaseId { get; }
        public string Assignee { get; }
        public DateTime AssignedAt { get; }
    }

}
