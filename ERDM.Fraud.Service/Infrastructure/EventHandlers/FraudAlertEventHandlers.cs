using ERDM.Fraud.Service.Domain.Events;
using ERDM.Fraud.Service.Infrastructure.ReadModels;
using MediatR;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Infrastructure.EventHandlers
{
    public class FraudAlertCreatedEventHandler : INotificationHandler<FraudAlertCreatedEvent>
    {
        private readonly IMongoCollection<FraudAlertReadModel> _readCollection;

        public FraudAlertCreatedEventHandler(IMongoDatabase database)
        {
            _readCollection = database.GetCollection<FraudAlertReadModel>("fraud_alerts_read");
        }

        public async Task Handle(FraudAlertCreatedEvent notification, CancellationToken cancellationToken)
        {
            var readModel = new FraudAlertReadModel
            {
                Id = notification.EntityId,
                AlertId = notification.AlertId,
                CustomerId = notification.CustomerId,
                AlertType = notification.AlertType,
                Severity = notification.Severity,
                Description = notification.Description,
                IsResolved = false,
                CreatedAt = notification.CreatedAt
            };

            await _readCollection.InsertOneAsync(readModel, cancellationToken: cancellationToken);
        }
    }

    public class FraudAlertResolvedEventHandler : INotificationHandler<FraudAlertResolvedEvent>
    {
        private readonly IMongoCollection<FraudAlertReadModel> _readCollection;

        public FraudAlertResolvedEventHandler(IMongoDatabase database)
        {
            _readCollection = database.GetCollection<FraudAlertReadModel>("fraud_alerts_read");
        }

        public async Task Handle(FraudAlertResolvedEvent notification, CancellationToken cancellationToken)
        {
            var filter = Builders<FraudAlertReadModel>.Filter.Eq(x => x.AlertId, notification.AlertId);
            var update = Builders<FraudAlertReadModel>.Update
                .Set(x => x.IsResolved, true)
                .Set(x => x.ResolvedAt, notification.ResolvedAt);

            await _readCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        }
    }
}
