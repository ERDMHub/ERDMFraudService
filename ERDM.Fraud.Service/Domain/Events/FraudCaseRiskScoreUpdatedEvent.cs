using ERDM.Core.Entities;
using ERDM.Fraud.Service.Domain.Entities;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class FraudCaseRiskScoreUpdatedEvent : DomainEventBase
    {
        public FraudCaseRiskScoreUpdatedEvent(FraudCase fraudCase, int newScore)
        {
            EntityId = fraudCase.Id;
            EntityType = nameof(FraudCase);
            CaseId = fraudCase.CaseId;
            NewScore = newScore;
            UpdatedAt = DateTime.UtcNow;
        }

        public string CaseId { get; }
        public int NewScore { get; }
        public DateTime UpdatedAt { get; }
    }
}
