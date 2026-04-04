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
    public class GetDeviceByIdQueryHandler : IRequestHandler<GetDeviceByIdQuery, ApiResponse<DeviceFingerprintResponseDto>>
    {
        private readonly IMongoCollection<DeviceFingerprintReadModel> _collection;
        private readonly IMapper _mapper;
        private readonly ILogger<GetDeviceByIdQueryHandler> _logger;

        public GetDeviceByIdQueryHandler(
            IMongoDatabase database,
            IOptions<MongoDbSettings> settings,
            IMapper mapper,
            ILogger<GetDeviceByIdQueryHandler> logger)
        {
            _collection = database.GetCollection<DeviceFingerprintReadModel>("device_fingerprints_read");
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ApiResponse<DeviceFingerprintResponseDto>> Handle(GetDeviceByIdQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var filter = Builders<DeviceFingerprintReadModel>.Filter.Eq(x => x.DeviceId, query.DeviceId);
                var result = await _collection.Find(filter).FirstOrDefaultAsync(cancellationToken);

                if (result == null)
                    return ApiResponse<DeviceFingerprintResponseDto>.Fail($"Device {query.DeviceId} not found");

                var response = _mapper.Map<DeviceFingerprintResponseDto>(result);
                return ApiResponse<DeviceFingerprintResponseDto>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting device by ID {DeviceId}", query.DeviceId);
                return ApiResponse<DeviceFingerprintResponseDto>.Fail(ex.Message);
            }
        }
    }
}
