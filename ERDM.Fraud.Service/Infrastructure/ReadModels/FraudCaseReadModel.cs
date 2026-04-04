namespace ERDM.Fraud.Service.Infrastructure.ReadModels
{
    public class FraudCaseReadModel
    {
        public string Id { get; set; } = string.Empty;
        public string CaseId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string FraudType { get; set; } = string.Empty;
        public int RiskScore { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? AssignedTo { get; set; }
        public string? AssignedToName { get; set; }

        // Evidence Summary
        public int EvidenceCount { get; set; }
        public List<string> EvidenceTypes { get; set; } = new();
        public DateTime? LastEvidenceAddedAt { get; set; }

        // Linked Cases
        public List<string> LinkedCases { get; set; } = new();
        public int LinkedCasesCount { get; set; }

        // Network Analysis
        public List<FraudNetworkNodeReadModel> NetworkNodes { get; set; } = new();
        public int NetworkSize { get; set; }

        // Resolution
        public string? ResolutionType { get; set; }
        public string? ResolutionDescription { get; set; }
        public string? ActionTaken { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public string? ResolvedBy { get; set; }

        // Dates
        public DateTime OpenedAt { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public DateTime? EscalatedAt { get; set; }

        // Customer Information (denormalized for quick access)
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;

        // Device Information
        public List<string> AssociatedDeviceIds { get; set; } = new();
        public int DeviceCount { get; set; }

        // Application Information
        public List<string> RelatedApplicationIds { get; set; } = new();
        public int ApplicationCount { get; set; }
        public decimal? TotalRequestedAmount { get; set; }

        // Fraud Indicators
        public List<FraudIndicatorReadModel> FraudIndicators { get; set; } = new();
        public int IndicatorCount { get; set; }
        public decimal ConfidenceScore { get; set; }

        // AML/Sanctions
        public bool HasSanctionsHit { get; set; }
        public List<SanctionHitReadModel> SanctionsHits { get; set; } = new();

        // Synthetic Identity
        public bool IsSyntheticIdentity { get; set; }
        public decimal? SyntheticIdentityScore { get; set; }

        // Timeline
        public List<FraudCaseTimelineReadModel> Timeline { get; set; } = new();

        // Notes and Comments
        public List<FraudCaseNoteReadModel> Notes { get; set; } = new();
        public int NoteCount { get; set; }

        // Attachments
        public List<FraudCaseAttachmentReadModel> Attachments { get; set; } = new();

        // Risk Assessment
        public FraudRiskAssessmentReadModel? RiskAssessment { get; set; }

        // Workflow
        public string CurrentWorkflowStep { get; set; } = string.Empty;
        public List<string> WorkflowHistory { get; set; } = new();
        public DateTime? ExpectedResolutionDate { get; set; }

        // SLA Tracking
        public DateTime? SLAExpiresAt { get; set; }
        public bool IsSLAExceeded { get; set; }
        public int? SLADaysRemaining { get; set; }

        // Audit
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }

        // Metadata
        public Dictionary<string, object> Metadata { get; set; } = new();
        public List<string> Tags { get; set; } = new();

        // Statistics for Dashboard
        public int ReviewCount { get; set; }
        public int EscalationCount { get; set; }
        public TimeSpan? TimeToResolution { get; set; }

        // Flag for UI
        public bool IsUrgent { get; set; }
        public bool RequiresManagerApproval { get; set; }
        public bool IsHighProfile { get; set; }
    }
}
