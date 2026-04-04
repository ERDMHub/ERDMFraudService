using ERDM.Fraud.Service.Domain.Common;
using ERDM.Fraud.Service.Domain.Enums;
using ERDM.Fraud.Service.Domain.Events;
using ERDM.Fraud.Service.Domain.ValueObjects;
using MongoDB.Bson.Serialization.Attributes;

namespace ERDM.Fraud.Service.Domain.Entities
{
    public class DeviceFingerprint : AggregateRoot
    {
        [BsonElement("deviceId")]
        public string DeviceId { get; private set; }

        [BsonElement("fingerprintHash")]
        public string FingerprintHash { get; private set; }

        [BsonElement("customerId")]
        public string? CustomerId { get; private set; }

        [BsonElement("deviceType")]
        public DeviceType DeviceType { get; private set; }

        [BsonElement("operatingSystem")]
        public string OperatingSystem { get; private set; }

        [BsonElement("browserInfo")]
        public BrowserInfo BrowserInfo { get; private set; }

        [BsonElement("hardwareInfo")]
        public HardwareInfo HardwareInfo { get; private set; }

        [BsonElement("networkInfo")]
        public NetworkInfo NetworkInfo { get; private set; }

        [BsonElement("riskScore")]
        public int RiskScore { get; private set; }

        [BsonElement("riskLevel")]
        public RiskLevel RiskLevel { get; private set; }

        [BsonElement("isBlacklisted")]
        public bool IsBlacklisted { get; private set; }

        [BsonElement("blacklistReason")]
        public string? BlacklistReason { get; private set; }

        [BsonElement("firstSeenAt")]
        public DateTime FirstSeenAt { get; private set; }

        [BsonElement("lastSeenAt")]
        public DateTime LastSeenAt { get; private set; }

        [BsonElement("applicationsCount")]
        public int ApplicationsCount { get; private set; }

        [BsonElement("associatedCustomers")]
        public List<string> AssociatedCustomers { get; private set; }

        [BsonElement("behavioralPatterns")]
        public List<BehavioralPattern> BehavioralPatterns { get; private set; }

        // Public parameterless constructor for MongoDB deserialization
        public DeviceFingerprint()
        {
            AssociatedCustomers = new List<string>();
            BehavioralPatterns = new List<BehavioralPattern>();
            BrowserInfo = new BrowserInfo();
            HardwareInfo = new HardwareInfo();
            NetworkInfo = new NetworkInfo();
        }

        public static DeviceFingerprint Create(
            string fingerprintHash,
            DeviceType deviceType,
            string operatingSystem,
            BrowserInfo browserInfo,
            HardwareInfo hardwareInfo,
            NetworkInfo networkInfo)
        {
            var device = new DeviceFingerprint
            {
                DeviceId = GenerateDeviceId(),
                FingerprintHash = fingerprintHash,
                DeviceType = deviceType,
                OperatingSystem = operatingSystem,
                BrowserInfo = browserInfo,
                HardwareInfo = hardwareInfo,
                NetworkInfo = networkInfo,
                FirstSeenAt = DateTime.UtcNow,
                LastSeenAt = DateTime.UtcNow,
                AssociatedCustomers = new List<string>(),
                BehavioralPatterns = new List<BehavioralPattern>(),
                RiskScore = 0,
                RiskLevel = RiskLevel.Unknown,
                IsBlacklisted = false
            };

            device.AddDomainEvent(new DeviceRegisteredEvent(device));
            return device;
        }

        public void AssociateWithCustomer(string customerId)
        {
            if (!AssociatedCustomers.Contains(customerId))
            {
                AssociatedCustomers.Add(customerId);
                ApplicationsCount++;
                LastSeenAt = DateTime.UtcNow;
                CustomerId = customerId;

                AddDomainEvent(new DeviceAssociatedEvent(this, customerId));
            }
        }

        public void UpdateRiskScore(int score)
        {
            RiskScore = score;
            RiskLevel = CalculateRiskLevel(score);
            LastSeenAt = DateTime.UtcNow;

            AddDomainEvent(new DeviceRiskScoreUpdatedEvent(this, score, RiskLevel));
        }

        public void Blacklist(string reason)
        {
            IsBlacklisted = true;
            BlacklistReason = reason;
            RiskLevel = RiskLevel.VeryHigh;

            AddDomainEvent(new DeviceBlacklistedEvent(this, reason));
        }

        public void AddBehavioralPattern(BehavioralPattern pattern)
        {
            BehavioralPatterns.Add(pattern);
            AddDomainEvent(new BehavioralPatternDetectedEvent(this, pattern));
        }

        private static RiskLevel CalculateRiskLevel(int score)
        {
            return score switch
            {
                >= 80 => RiskLevel.VeryHigh,
                >= 60 => RiskLevel.High,
                >= 40 => RiskLevel.Medium,
                >= 20 => RiskLevel.Low,
                _ => RiskLevel.VeryLow
            };
        }

        private static string GenerateDeviceId()
        {
            return $"DEV-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid():N}";
        }
    }
}