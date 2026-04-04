using MediatR;

namespace ERDM.Fraud.Service.Application.Behaviors
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var requestId = Guid.NewGuid().ToString();

            _logger.LogInformation(
                "Processing {RequestName} [RequestId: {RequestId}] at {Time}",
                requestName,
                requestId,
                DateTime.UtcNow);

            try
            {
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                var response = await next();
                stopwatch.Stop();

                _logger.LogInformation(
                    "Completed {RequestName} [RequestId: {RequestId}] in {ElapsedMilliseconds}ms at {Time}",
                    requestName,
                    requestId,
                    stopwatch.ElapsedMilliseconds,
                    DateTime.UtcNow);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error processing {RequestName} [RequestId: {RequestId}] at {Time}",
                    requestName,
                    requestId,
                    DateTime.UtcNow);
                throw;
            }
        }
    }
}
