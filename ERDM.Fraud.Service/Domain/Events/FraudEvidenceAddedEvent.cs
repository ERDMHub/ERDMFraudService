using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.ValueObjects;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class FraudEvidenceAddedEvent : FraudDomainEventBase
    {
        public FraudEvidenceAddedEvent(FraudCase fraudCase, FraudEvidence evidence)
        {
            EntityId = fraudCase.Id;
            EntityType = nameof(FraudCase);
            CaseId = fraudCase.CaseId;
            EvidenceId = evidence.EvidenceId;
            EvidenceType = evidence.EvidenceType;
            AddedAt = evidence.CollectedAt;
        }

        public string CaseId { get; }
        public string EvidenceId { get; }
        public string EvidenceType { get; }
        public DateTime AddedAt { get; }
    }

}
