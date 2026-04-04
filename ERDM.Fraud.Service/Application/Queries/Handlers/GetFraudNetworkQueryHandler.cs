using AutoMapper;
using ERDM.Fraud.Service.Contracts.Dtos;
using ERDM.Fraud.Service.Contracts.Wrapper;
using ERDM.Fraud.Service.Infrastructure.ReadModels;
using ERDMCore.Infrastructure.MongoDB.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Application.Queries.Handlers
{
    public class GetFraudNetworkQueryHandler : IRequestHandler<GetFraudNetworkQuery, ApiResponse<FraudNetworkResponseDto>>
    {
        private readonly IMongoCollection<FraudCaseReadModel> _collection;
        private readonly IMapper _mapper;
        private readonly ILogger<GetFraudNetworkQueryHandler> _logger;

        public GetFraudNetworkQueryHandler(
            IMongoDatabase database,
            IOptions<MongoDbSettings> settings,
            IMapper mapper,
            ILogger<GetFraudNetworkQueryHandler> logger)
        {
            _collection = database.GetCollection<FraudCaseReadModel>("fraud_cases_read");
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<FraudNetworkResponseDto>> Handle(GetFraudNetworkQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var visited = new HashSet<string>();
                var network = new FraudNetworkResponseDto
                {
                    Nodes = new List<FraudNetworkNodeDto>(),
                    Edges = new List<FraudNetworkEdgeDto>()
                };

                await BuildNetworkGraph(query.CustomerId, network, visited, query.Depth, cancellationToken);

                return ApiResponse<FraudNetworkResponseDto>.Ok(network);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error building fraud network for customer {CustomerId}", query.CustomerId);
                return ApiResponse<FraudNetworkResponseDto>.Fail(ex.Message);
            }
        }

        private async Task BuildNetworkGraph(string customerId, FraudNetworkResponseDto network, HashSet<string> visited, int depthRemaining, CancellationToken cancellationToken)
        {
            if (depthRemaining <= 0 || visited.Contains(customerId))
                return;

            visited.Add(customerId);

            var filter = Builders<FraudCaseReadModel>.Filter.Eq(x => x.CustomerId, customerId);
            var cases = await _collection.Find(filter).ToListAsync(cancellationToken);

            foreach (var fraudCase in cases)
            {
                // Add node
                network.Nodes.Add(new FraudNetworkNodeDto
                {
                    Id = customerId,
                    Type = "Customer",
                    RiskScore = fraudCase.RiskScore
                });

                // Process linked cases
                foreach (var linkedCaseId in fraudCase.LinkedCases)
                {
                    network.Edges.Add(new FraudNetworkEdgeDto
                    {
                        Source = customerId,
                        Target = linkedCaseId,
                        Relationship = "LinkedCase"
                    });

                    await BuildNetworkGraph(linkedCaseId, network, visited, depthRemaining - 1, cancellationToken);
                }
            }
        }
    }
}
