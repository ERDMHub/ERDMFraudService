using ERDM.Fraud.Service.Domain.Events;
using ERDM.Fraud.Service.Infrastructure.ReadModels;
using MediatR;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Infrastructure.EventHandlers
{
    public class DeviceEventHandlers :
        INotificationHandler<DeviceRegisteredEvent>,
        INotificationHandler<DeviceAssociatedEvent>,
        INotificationHandler<DeviceRiskScoreUpdatedEvent>,
        INotificationHandler<DeviceBlacklistedEvent>,
        INotificationHandler<BehavioralPatternDetectedEvent>  // Add this line
    {
        private readonly ILogger<DeviceEventHandlers> _logger;
        private readonly IMongoCollection<DeviceFingerprintReadModel> _collection;

        public DeviceEventHandlers(ILogger<DeviceEventHandlers> logger, IMongoDatabase database)
        {
            _logger = logger;
            _collection = database.GetCollection<DeviceFingerprintReadModel>("device_fingerprints_read");
        }

        public Task Handle(DeviceRegisteredEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Device registered: {DeviceId}, Fingerprint: {Fingerprint}",
                notification.DeviceId, notification.FingerprintHash);
            return Task.CompletedTask;
        }

        public Task Handle(DeviceAssociatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Device {DeviceId} associated with customer {CustomerId}",
                notification.DeviceId, notification.CustomerId);
            return Task.CompletedTask;
        }

        public Task Handle(DeviceRiskScoreUpdatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Device {DeviceId} risk score updated to {Score} ({RiskLevel})",
                notification.DeviceId, notification.NewScore, notification.RiskLevel);
            return Task.CompletedTask;
        }

        public Task Handle(DeviceBlacklistedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Device {DeviceId} blacklisted. Reason: {Reason}",
                notification.DeviceId, notification.Reason);
            return Task.CompletedTask;
        }

        public Task Handle(BehavioralPatternDetectedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Behavioral pattern detected on device {DeviceId}: {PatternType} = {PatternValue} (Confidence: {Confidence})",
                notification.DeviceId, notification.PatternType, notification.PatternValue, notification.Confidence);
            return Task.CompletedTask;
        }
    }
}
