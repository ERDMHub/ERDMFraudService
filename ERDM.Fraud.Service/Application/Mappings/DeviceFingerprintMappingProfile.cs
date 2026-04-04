using AutoMapper;
using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Domain.Entities;
using ERDM.Fraud.Service.Domain.Enums;
using ERDM.Fraud.Service.Domain.ValueObjects;

namespace ERDM.Fraud.Service.Application.Mappings
{
    public class DeviceFingerprintMappingProfile : Profile
    {
        public DeviceFingerprintMappingProfile()
        {
            // DTO to Entity (for Commands)
            CreateMap<RegisterDeviceCommand, DeviceFingerprint>()
                .ForMember(dest => dest.DeviceId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Version, opt => opt.Ignore())
                .ForMember(dest => dest.FirstSeenAt, opt => opt.Ignore())
                .ForMember(dest => dest.LastSeenAt, opt => opt.Ignore())
                .ForMember(dest => dest.RiskScore, opt => opt.Ignore())
                .ForMember(dest => dest.RiskLevel, opt => opt.Ignore())
                .ForMember(dest => dest.IsBlacklisted, opt => opt.Ignore())
                .ForMember(dest => dest.BlacklistReason, opt => opt.Ignore())
                .ForMember(dest => dest.AssociatedCustomers, opt => opt.Ignore())
                .ForMember(dest => dest.BehavioralPatterns, opt => opt.Ignore())
                .ForMember(dest => dest.ApplicationsCount, opt => opt.Ignore())
                .ForMember(dest => dest.CustomerId, opt => opt.Ignore())
                .ForMember(dest => dest.DeviceType, opt => opt.MapFrom(src => Enum.Parse<DeviceType>(src.DeviceType)))
                .ForMember(dest => dest.BrowserInfo, opt => opt.MapFrom(src => src.BrowserInfo))
                .ForMember(dest => dest.HardwareInfo, opt => opt.MapFrom(src => src.HardwareInfo))
                .ForMember(dest => dest.NetworkInfo, opt => opt.MapFrom(src => src.NetworkInfo));

            // DTO to Value Objects
            CreateMap<BrowserInfoDto, BrowserInfo>()
                .ForMember(dest => dest.UserAgent, opt => opt.MapFrom(src => src.UserAgent))
                .ForMember(dest => dest.BrowserName, opt => opt.MapFrom(src => src.BrowserName))
                .ForMember(dest => dest.BrowserVersion, opt => opt.MapFrom(src => src.BrowserVersion))
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.CookiesEnabled, opt => opt.MapFrom(src => src.CookiesEnabled))
                .ForMember(dest => dest.JavascriptEnabled, opt => opt.MapFrom(src => src.JavascriptEnabled))
                .ForMember(dest => dest.ScreenResolution, opt => opt.MapFrom(src => src.ScreenResolution))
                .ForMember(dest => dest.Timezone, opt => opt.MapFrom(src => src.Timezone));

            CreateMap<HardwareInfoDto, HardwareInfo>()
                .ForMember(dest => dest.DeviceModel, opt => opt.MapFrom(src => src.DeviceModel))
                .ForMember(dest => dest.DeviceBrand, opt => opt.MapFrom(src => src.DeviceBrand))
                .ForMember(dest => dest.CpuCores, opt => opt.MapFrom(src => src.CpuCores))
                .ForMember(dest => dest.RamSize, opt => opt.MapFrom(src => src.RamSize))
                .ForMember(dest => dest.GraphicsCard, opt => opt.MapFrom(src => src.GraphicsCard))
                .ForMember(dest => dest.IsEmulator, opt => opt.MapFrom(src => src.IsEmulator))
                .ForMember(dest => dest.IsRooted, opt => opt.MapFrom(src => src.IsRooted));

            CreateMap<NetworkInfoDto, NetworkInfo>()
                .ForMember(dest => dest.IpAddress, opt => opt.MapFrom(src => src.IpAddress))
                .ForMember(dest => dest.MacAddress, opt => opt.MapFrom(src => src.MacAddress))
                .ForMember(dest => dest.Isp, opt => opt.MapFrom(src => src.Isp))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Country))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.IsProxy, opt => opt.MapFrom(src => src.IsProxy))
                .ForMember(dest => dest.IsVpn, opt => opt.MapFrom(src => src.IsVpn))
                .ForMember(dest => dest.IsTor, opt => opt.MapFrom(src => src.IsTor));

            // Entity to Response DTO
            CreateMap<DeviceFingerprint, DeviceFingerprintResponseDto>()
                .ForMember(dest => dest.DeviceType, opt => opt.MapFrom(src => src.DeviceType.ToString()))
                .ForMember(dest => dest.RiskLevel, opt => opt.MapFrom(src => src.RiskLevel.ToString()))
                .ForMember(dest => dest.BrowserInfo, opt => opt.MapFrom(src => src.BrowserInfo))
                .ForMember(dest => dest.HardwareInfo, opt => opt.MapFrom(src => src.HardwareInfo))
                .ForMember(dest => dest.NetworkInfo, opt => opt.MapFrom(src => src.NetworkInfo))
                .ForMember(dest => dest.BehavioralPatterns, opt => opt.MapFrom(src => src.BehavioralPatterns));

            // Value Object to DTO
            CreateMap<BrowserInfo, BrowserInfoDto>();
            CreateMap<HardwareInfo, HardwareInfoDto>();
            CreateMap<NetworkInfo, NetworkInfoDto>();
            CreateMap<BehavioralPattern, BehavioralPatternDto>();
        }
    }
}