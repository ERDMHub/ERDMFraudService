using MediatR;
using System.Diagnostics;

namespace ERDM.Fraud.Service.Application.Behaviors
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly Stopwatch _timer;
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
        private readonly int _slowRequestThresholdMs = 500;

        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
        {
            _timer = new Stopwatch();
            _logger = logger;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            _timer.Start();
            var response = await next();
            _timer.Stop();

            var elapsedMilliseconds = _timer.ElapsedMilliseconds;

            if (elapsedMilliseconds > _slowRequestThresholdMs)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogWarning(
                    "Slow Request detected: {RequestName} took {ElapsedMilliseconds}ms (threshold: {Threshold}ms)",
                    requestName,
                    elapsedMilliseconds,
                    _slowRequestThresholdMs);
            }

            return response;
        }
    }
}
