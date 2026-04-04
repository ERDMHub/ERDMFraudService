using AutoMapper;
using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;
using ERDM.Fraud.Service.Domain.ValueObjects;

namespace ERDM.Fraud.Service.Application.Mappings
{
    public class IdentityVerificationMappingProfile : Profile
    {
        public IdentityVerificationMappingProfile()
        {
            // Command DTO to Entity (for Create/Initiate)
            CreateMap<InitiateIdentityVerificationCommand, IdentityVerification>()
                .ForMember(dest => dest.VerificationId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore())
                .ForMember(dest => dest.VerificationStatus, opt => opt.Ignore())
                .ForMember(dest => dest.VerificationScore, opt => opt.Ignore())
                .ForMember(dest => dest.VerificationDetails, opt => opt.Ignore())
                .ForMember(dest => dest.LivenessResult, opt => opt.Ignore())
                .ForMember(dest => dest.BiometricHash, opt => opt.Ignore())
                .ForMember(dest => dest.VerifiedAt, opt => opt.Ignore())
                .ForMember(dest => dest.VerifiedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ExpiresAt, opt => opt.Ignore())
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => Enum.Parse<DocumentType>(src.DocumentType)));

            // Complete Verification Command to Entity (for updates)
            CreateMap<CompleteIdentityVerificationCommand, IdentityVerification>()
                .ForMember(dest => dest.VerificationScore, opt => opt.MapFrom(src => src.VerificationScore))
                .ForMember(dest => dest.VerificationDetails, opt => opt.MapFrom(src => src.Details))
                .ForMember(dest => dest.VerifiedBy, opt => opt.MapFrom(src => src.VerifiedBy))
                .ForMember(dest => dest.VerifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.VerificationStatus, opt => opt.Ignore());

            // DTO to Value Objects
            CreateMap<VerificationDetailsDto, VerificationDetails>()
                .ForMember(dest => dest.DocumentAuthentic, opt => opt.MapFrom(src => src.DocumentAuthentic))
                .ForMember(dest => dest.FaceMatch, opt => opt.MapFrom(src => src.FaceMatch))
                .ForMember(dest => dest.FaceMatchScore, opt => opt.MapFrom(src => src.FaceMatchScore))
                .ForMember(dest => dest.DataConsistent, opt => opt.MapFrom(src => src.DataConsistent))
                .ForMember(dest => dest.Flags, opt => opt.MapFrom(src => src.Flags ?? new List<string>()))
                .ForMember(dest => dest.Warnings, opt => opt.MapFrom(src => src.Warnings ?? new List<string>()));

            CreateMap<LivenessResultDto, LivenessResult>()
                .ForMember(dest => dest.IsAlive, opt => opt.MapFrom(src => src.IsAlive))
                .ForMember(dest => dest.ConfidenceScore, opt => opt.MapFrom(src => src.ConfidenceScore))
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method))
                .ForMember(dest => dest.ChecksPassed, opt => opt.MapFrom(src => src.ChecksPassed ?? new List<string>()))
                .ForMember(dest => dest.ChecksFailed, opt => opt.MapFrom(src => src.ChecksFailed ?? new List<string>()));

            // Entity to Response DTO
            CreateMap<IdentityVerification, IdentityVerificationResponseDto>()
                .ForMember(dest => dest.DocumentType, opt => opt.MapFrom(src => src.DocumentType.ToString()))
                .ForMember(dest => dest.VerificationStatus, opt => opt.MapFrom(src => src.VerificationStatus.ToString()))
                .ForMember(dest => dest.VerificationDetails, opt => opt.MapFrom(src => src.VerificationDetails))
                .ForMember(dest => dest.LivenessResult, opt => opt.MapFrom(src => src.LivenessResult));

            // Value Object to DTO
            CreateMap<VerificationDetails, VerificationDetailsDto>();
            CreateMap<LivenessResult, LivenessResultDto>();

            // Register Biometric Command to Entity
            CreateMap<RegisterBiometricCommand, IdentityVerification>()
                .ForMember(dest => dest.BiometricHash, opt => opt.MapFrom(src => src.BiometricData))
                .ForMember(dest => dest.VerifiedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.VerifiedBy, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                .ForMember(dest => dest.VerificationStatus, opt => opt.MapFrom(src => VerificationStatus.Approved));
        }
    }
}