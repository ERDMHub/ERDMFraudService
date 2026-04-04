using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;

namespace ERDM.Fraud.Service.Domain.Events
{
    // Fraud Case Events
    public class FraudCaseOpenedEvent : FraudDomainEventBase
    {
        public FraudCaseOpenedEvent(FraudCase fraudCase)
        {
            EntityId = fraudCase.Id;
            EntityType = nameof(FraudCase);
            CaseId = fraudCase.CaseId;
            CustomerId = fraudCase.CustomerId;
            FraudType = fraudCase.FraudType;
            RiskScore = fraudCase.RiskScore;
            OpenedAt = DateTime.UtcNow;
        }

        public string CaseId { get; }
        public string CustomerId { get; }
        public FraudType FraudType { get; }
        public int RiskScore { get; }
        public DateTime OpenedAt { get; }
    }
}
