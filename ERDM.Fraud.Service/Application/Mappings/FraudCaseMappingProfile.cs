using AutoMapper;
using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;
using ERDM.Fraud.Service.Domain.ValueObjects;

namespace ERDM.Fraud.Service.Application.Mappings
{
    public class FraudCaseMappingProfile : Profile
    {
        public FraudCaseMappingProfile()
        {
            // Command DTO to Entity (for Create/Open)
            CreateMap<OpenFraudCaseCommand, FraudCase>()
                .ForMember(dest => dest.CaseId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Evidence, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedTo, opt => opt.Ignore())
                .ForMember(dest => dest.Resolution, opt => opt.Ignore())
                .ForMember(dest => dest.LinkedCases, opt => opt.Ignore())
                .ForMember(dest => dest.NetworkNodes, opt => opt.Ignore())
                .ForMember(dest => dest.FraudType, opt => opt.MapFrom(src => Enum.Parse<FraudType>(src.FraudType)));

            // Add Evidence Command to Entity
            CreateMap<AddFraudEvidenceCommand, FraudEvidence>()
                .ForMember(dest => dest.EvidenceId, opt => opt.Ignore())
                .ForMember(dest => dest.CollectedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata ?? new Dictionary<string, object>()));

            // Link Cases Command
            CreateMap<LinkFraudCasesCommand, FraudCase>()
                .ForMember(dest => dest.LinkedCases, opt => opt.MapFrom(src => new List<string> { src.PrimaryCaseId, src.SecondaryCaseId }));

            // Resolve Case Command
            CreateMap<ResolveFraudCaseCommand, FraudResolution>()
                .ForMember(dest => dest.ResolvedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.ResolvedBy, opt => opt.MapFrom(src => src.ResolvedBy))
                .ForMember(dest => dest.ActionTaken, opt => opt.MapFrom(src => src.ActionTaken))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.ResolutionType, opt => opt.MapFrom(src => src.ResolutionType));

            // Assign Case Command
            CreateMap<AssignFraudCaseCommand, FraudCase>()
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.AssigneeId));

            // Update Risk Score Command
            CreateMap<UpdateFraudCaseRiskScoreCommand, FraudCase>()
                .ForMember(dest => dest.RiskScore, opt => opt.MapFrom(src => src.NewRiskScore));

            // DTO to Value Objects
            CreateMap<FraudEvidenceDto, FraudEvidence>()
                .ForMember(dest => dest.EvidenceId, opt => opt.MapFrom(src => src.EvidenceId ?? Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CollectedAt, opt => opt.MapFrom(src => src.CollectedAt))
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata ?? new Dictionary<string, object>()));

            CreateMap<FraudResolutionDto, FraudResolution>()
                .ForMember(dest => dest.ResolvedAt, opt => opt.MapFrom(src => src.ResolvedAt))
                .ForMember(dest => dest.ResolvedBy, opt => opt.MapFrom(src => src.ResolvedBy));

            CreateMap<FraudNetworkNodeDto, NetworkNode>()
                .ForMember(dest => dest.NodeId, opt => opt.Ignore())
                .ForMember(dest => dest.NodeType, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.ConnectionStrength, opt => opt.MapFrom(src => src.RiskScore / 100m))
                .ForMember(dest => dest.Connections, opt => opt.Ignore());

            // Entity to Response DTO
            CreateMap<FraudCase, FraudCaseResponseDto>()
                .ForMember(dest => dest.FraudType, opt => opt.MapFrom(src => src.FraudType.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Evidence, opt => opt.MapFrom(src => src.Evidence))
                .ForMember(dest => dest.Resolution, opt => opt.MapFrom(src => src.Resolution))
                .ForMember(dest => dest.LinkedCases, opt => opt.MapFrom(src => src.LinkedCases))
                .ForMember(dest => dest.NetworkNodes, opt => opt.MapFrom(src => src.NetworkNodes))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            // Value Object to DTO
            CreateMap<FraudEvidence, FraudEvidenceDto>()
                .ForMember(dest => dest.Metadata, opt => opt.MapFrom(src => src.Metadata ?? new Dictionary<string, object>()));

            CreateMap<FraudResolution, FraudResolutionDto>();
            CreateMap<NetworkNode, FraudNetworkNodeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.NodeId))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.NodeType))
                .ForMember(dest => dest.RiskScore, opt => opt.MapFrom(src => (int)(src.ConnectionStrength * 100)));

            // Statistics DTO mappings
            CreateMap<FraudCase, FraudStatisticsDto>()
                .ForMember(dest => dest.TotalFraudCases, opt => opt.MapFrom(src => 1))
                .ForMember(dest => dest.CasesByType, opt => opt.Ignore())
                .ForMember(dest => dest.CasesByStatus, opt => opt.Ignore());
        }
    }
}