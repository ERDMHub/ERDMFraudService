using AutoMapper;
using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Infrastructure.ReadModels;

namespace ERDM.Fraud.Service.Application.Mappings
{
    public class FraudAlertMappingProfile : Profile
    {
        public FraudAlertMappingProfile()
        {
            // Entity to Response DTO
            CreateMap<FraudAlert, FraudAlertResponseDto>()
                .ForMember(dest => dest.AlertId, opt => opt.MapFrom(src => src.AlertId))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.AlertType, opt => opt.MapFrom(src => src.AlertType))
                .ForMember(dest => dest.Severity, opt => opt.MapFrom(src => src.Severity))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsResolved, opt => opt.MapFrom(src => src.IsResolved))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ResolvedAt, opt => opt.MapFrom(src => src.ResolvedAt));

            // Command to Entity (for Create)
            CreateMap<CreateFraudAlertCommand, FraudAlert>()
                .ForMember(dest => dest.AlertId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore())
                .ForMember(dest => dest.IsResolved, opt => opt.Ignore())
                .ForMember(dest => dest.ResolvedAt, opt => opt.Ignore())
                .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

            // Entity to ReadModel (for projections)
            CreateMap<FraudAlert, FraudAlertReadModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AlertId, opt => opt.MapFrom(src => src.AlertId))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.AlertType, opt => opt.MapFrom(src => src.AlertType))
                .ForMember(dest => dest.Severity, opt => opt.MapFrom(src => src.Severity))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsResolved, opt => opt.MapFrom(src => src.IsResolved))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ResolvedAt, opt => opt.MapFrom(src => src.ResolvedAt));
        }
    }
}
