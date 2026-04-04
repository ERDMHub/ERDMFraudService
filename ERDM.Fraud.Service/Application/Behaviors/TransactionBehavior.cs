using MediatR;
using MongoDB.Driver;

namespace ERDM.Fraud.Service.Application.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
       where TRequest : IRequest<TResponse>
    {
        private readonly IMongoClient _mongoClient;
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

        public TransactionBehavior(
            IMongoClient mongoClient,
            ILogger<TransactionBehavior<TRequest, TResponse>> logger)
        {
            _mongoClient = mongoClient;
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            // Only wrap commands in transactions, not queries
            var isCommand = request.GetType().Name.EndsWith("Command");

            if (!isCommand)
                return await next();

            using var session = await _mongoClient.StartSessionAsync(cancellationToken: cancellationToken);
            session.StartTransaction();

            try
            {
                var response = await next();
                await session.CommitTransactionAsync(cancellationToken);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Transaction failed for {RequestType}, rolling back", typeof(TRequest).Name);
                await session.AbortTransactionAsync(cancellationToken);
                throw;
            }
        }
    }
}
