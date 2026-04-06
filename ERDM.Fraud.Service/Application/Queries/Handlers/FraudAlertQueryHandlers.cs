using AutoMapper;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using ERDM.Fraud.Service.Infrastructure.ReadModels;
using MediatR;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Application.Queries.Handlers
{
    public class GetFraudAlertByIdQueryHandler : IRequestHandler<GetFraudAlertByIdQuery, ApiResponse<FraudAlertResponseDto>>
    {
        private readonly IMongoCollection<FraudAlertReadModel> _readCollection;

        public GetFraudAlertByIdQueryHandler(IMongoDatabase database)
        {
            _readCollection = database.GetCollection<FraudAlertReadModel>("fraud_alerts_read");
        }

        public async Task<ApiResponse<FraudAlertResponseDto>> Handle(GetFraudAlertByIdQuery query, CancellationToken cancellationToken)
        {
            var filter = Builders<FraudAlertReadModel>.Filter.Eq(x => x.AlertId, query.AlertId);
            var result = await _readCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);

            if (result == null)
                return ApiResponse<FraudAlertResponseDto>.Fail($"Alert {query.AlertId} not found");

            var response = new FraudAlertResponseDto
            {
                AlertId = result.AlertId,
                CustomerId = result.CustomerId,
                AlertType = result.AlertType,
                Severity = result.Severity,
                Description = result.Description,
                IsResolved = result.IsResolved,
                CreatedAt = result.CreatedAt,
                ResolvedAt = result.ResolvedAt
            };

            return ApiResponse<FraudAlertResponseDto>.Ok(response);
        }
    }

    // Handler for GetFraudAlertsByCustomerQuery
    public class GetFraudAlertsByCustomerQueryHandler : IRequestHandler<GetFraudAlertsByCustomerQuery, ApiResponse<List<FraudAlertResponseDto>>>
    {
        private readonly IMongoCollection<FraudAlertReadModel> _readCollection;

        public GetFraudAlertsByCustomerQueryHandler(IMongoDatabase database)
        {
            _readCollection = database.GetCollection<FraudAlertReadModel>("fraud_alerts_read");
        }

        public async Task<ApiResponse<List<FraudAlertResponseDto>>> Handle(GetFraudAlertsByCustomerQuery query, CancellationToken cancellationToken)
        {
            var filter = Builders<FraudAlertReadModel>.Filter.Eq(x => x.CustomerId, query.CustomerId);
            var results = await _readCollection.Find(filter).ToListAsync(cancellationToken);

            var response = results.Select(r => new FraudAlertResponseDto
            {
                AlertId = r.AlertId,
                CustomerId = r.CustomerId,
                AlertType = r.AlertType,
                Severity = r.Severity,
                Description = r.Description,
                IsResolved = r.IsResolved,
                CreatedAt = r.CreatedAt,
                ResolvedAt = r.ResolvedAt
            }).ToList();

            return ApiResponse<List<FraudAlertResponseDto>>.Ok(response);
        }
    }

    // Handler for GetUnresolvedAlertsQuery
    public class GetUnresolvedAlertsQueryHandler : IRequestHandler<GetUnresolvedAlertsQuery, ApiResponse<List<FraudAlertResponseDto>>>
    {
        private readonly IMongoCollection<FraudAlertReadModel> _readCollection;

        public GetUnresolvedAlertsQueryHandler(IMongoDatabase database)
        {
            _readCollection = database.GetCollection<FraudAlertReadModel>("fraud_alerts_read");
        }

        public async Task<ApiResponse<List<FraudAlertResponseDto>>> Handle(GetUnresolvedAlertsQuery query, CancellationToken cancellationToken)
        {
            var filterBuilder = Builders<FraudAlertReadModel>.Filter;
            var filter = filterBuilder.Eq(x => x.IsResolved, false);

            if (!string.IsNullOrEmpty(query.Severity))
                filter = filterBuilder.And(filter, filterBuilder.Eq(x => x.Severity, query.Severity));

            var results = await _readCollection.Find(filter).ToListAsync(cancellationToken);

            var response = results.Select(r => new FraudAlertResponseDto
            {
                AlertId = r.AlertId,
                CustomerId = r.CustomerId,
                AlertType = r.AlertType,
                Severity = r.Severity,
                Description = r.Description,
                IsResolved = r.IsResolved,
                CreatedAt = r.CreatedAt,
                ResolvedAt = r.ResolvedAt
            }).ToList();

            return ApiResponse<List<FraudAlertResponseDto>>.Ok(response);
        }
    }

    public class GetAllFraudAlertsQueryHandler : IRequestHandler<GetAllFraudAlertsQuery, ApiResponse<List<FraudAlertResponseDto>>>
    {
        private readonly IMongoCollection<FraudAlertReadModel> _readCollection;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllFraudAlertsQueryHandler> _logger;

        public GetAllFraudAlertsQueryHandler(IMongoDatabase database, IMapper mapper, ILogger<GetAllFraudAlertsQueryHandler> logger)
        {
            _readCollection = database.GetCollection<FraudAlertReadModel>("fraud_alerts_read");
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<List<FraudAlertResponseDto>>> Handle(GetAllFraudAlertsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var filter = Builders<FraudAlertReadModel>.Filter.Empty;
                var sort = Builders<FraudAlertReadModel>.Sort.Descending(x => x.CreatedAt);

                var results = await _readCollection
                    .Find(filter)
                    .Sort(sort)
                    .ToListAsync(cancellationToken);

                var response = _mapper.Map<List<FraudAlertResponseDto>>(results);
                return ApiResponse<List<FraudAlertResponseDto>>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all fraud alerts");
                return ApiResponse<List<FraudAlertResponseDto>>.Fail(ex.Message);
            }
        }
    }
}
