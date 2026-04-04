using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.ValueObjects;

namespace ERDM.Fraud.Service.Domain.Events
{
    public class FraudNetworkNodeAddedEvent : FraudDomainEventBase
    {
        public FraudNetworkNodeAddedEvent(FraudCase fraudCase, NetworkNode node)
        {
            EntityId = fraudCase.Id;
            EntityType = nameof(FraudCase);
            CaseId = fraudCase.CaseId;
            NodeId = node.NodeId;
            NodeType = node.NodeType;
            AddedAt = DateTime.UtcNow;
        }

        public string CaseId { get; }
        public string NodeId { get; }
        public string NodeType { get; }
        public DateTime AddedAt { get; }
    }
}
