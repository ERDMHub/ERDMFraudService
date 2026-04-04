namespace ERDM.Fraud.Service.Contracts.Dtos
{
    public class DeviceFingerprintResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
        public string FingerprintHash { get; set; } = string.Empty;
        public string? CustomerId { get; set; }
        public string DeviceType { get; set; } = string.Empty;
        public string OperatingSystem { get; set; } = string.Empty;
        public BrowserInfoDto BrowserInfo { get; set; } = new();
        public HardwareInfoDto HardwareInfo { get; set; } = new();
        public NetworkInfoDto NetworkInfo { get; set; } = new();
        public int RiskScore { get; set; }
        public string RiskLevel { get; set; } = string.Empty;
        public bool IsBlacklisted { get; set; }
        public string? BlacklistReason { get; set; }
        public DateTime FirstSeenAt { get; set; }
        public DateTime LastSeenAt { get; set; }
        public int ApplicationsCount { get; set; }
        public List<string> AssociatedCustomers { get; set; } = new();
        public List<BehavioralPatternDto> BehavioralPatterns { get; set; } = new();
    }
}
