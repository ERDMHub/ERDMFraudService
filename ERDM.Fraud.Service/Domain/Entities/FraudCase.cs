using ERDM.Fraud.Service.Domain.Common;
using ERDM.Fraud.Service.Domain.Enums;
using ERDM.Fraud.Service.Domain.Events;
using ERDM.Fraud.Service.Domain.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Fraud.Service.Domain.Entities
{
    public class FraudCase : AggregateRoot
    {
        [BsonElement("caseId")]
        public string CaseId { get; private set; }

        [BsonElement("customerId")]
        public string CustomerId { get; private set; }

        [BsonElement("fraudType")]
        public FraudType FraudType { get; private set; }

        [BsonElement("riskScore")]
        public int RiskScore { get; private set; }

        [BsonElement("status")]
        public FraudCaseStatus Status { get; private set; }

        [BsonElement("evidence")]
        public List<FraudEvidence> Evidence { get; private set; }

        [BsonElement("assignedTo")]
        public string? AssignedTo { get; private set; }

        [BsonElement("resolution")]
        public FraudResolution? Resolution { get; private set; }

        [BsonElement("linkedCases")]
        public List<string> LinkedCases { get; private set; }

        [BsonElement("networkNodes")]
        public List<NetworkNode> NetworkNodes { get; private set; }

        // Public parameterless constructor for MongoDB deserialization
        public FraudCase()
        {
            Evidence = new List<FraudEvidence>();
            LinkedCases = new List<string>();
            NetworkNodes = new List<NetworkNode>();
        }

        public static FraudCase Create(string customerId, FraudType fraudType, int riskScore)
        {
            var fraudCase = new FraudCase
            {
                CaseId = GenerateCaseId(),
                CustomerId = customerId,
                FraudType = fraudType,
                RiskScore = riskScore,
                Status = FraudCaseStatus.Open,
                Evidence = new List<FraudEvidence>(),
                LinkedCases = new List<string>(),
                NetworkNodes = new List<NetworkNode>()
            };

            fraudCase.AddDomainEvent(new FraudCaseOpenedEvent(fraudCase));
            return fraudCase;
        }

        public void AddEvidence(FraudEvidence evidence)
        {
            Evidence.Add(evidence);
            AddDomainEvent(new FraudEvidenceAddedEvent(this, evidence));
        }

        public void LinkToCase(string caseId)
        {
            if (!LinkedCases.Contains(caseId))
            {
                LinkedCases.Add(caseId);
                AddDomainEvent(new FraudCaseLinkedEvent(this, caseId));
            }
        }

        public void AddToNetwork(NetworkNode node)
        {
            NetworkNodes.Add(node);
            AddDomainEvent(new FraudNetworkNodeAddedEvent(this, node));
        }

        public void Resolve(FraudResolution resolution)
        {
            Resolution = resolution;
            Status = FraudCaseStatus.Closed;
            AddDomainEvent(new FraudCaseResolvedEvent(this, resolution));
        }

        public void AssignTo(string assignee)
        {
            AssignedTo = assignee;
            AddDomainEvent(new FraudCaseAssignedEvent(this, assignee));
        }

        public void UpdateRiskScore(int newScore)
        {
            RiskScore = newScore;
            AddDomainEvent(new FraudCaseRiskScoreUpdatedEvent(this, newScore));
        }

        private static string GenerateCaseId()
        {
            return $"FRC-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid():N}";
        }
    }
}