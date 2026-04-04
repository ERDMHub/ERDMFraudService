using MediatR;
using Microsoft.AspNetCore.Mvc;
using ERDM.Fraud.Service.Application.Queries;

namespace ERDM.Fraud.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FraudAnalyticsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FraudAnalyticsController> _logger;

        public FraudAnalyticsController(IMediator mediator, ILogger<FraudAnalyticsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var query = new GetFraudStatisticsQuery { FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("dashboard/summary")]
        public async Task<IActionResult> GetDashboardSummary()
        {
            var query = new GetFraudDashboardSummaryQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("trends")]
        public async Task<IActionResult> GetFraudTrends([FromQuery] int months = 6)
        {
            var query = new GetFraudTrendsQuery { Months = months };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("top-fraud-types")]
        public async Task<IActionResult> GetTopFraudTypes([FromQuery] int limit = 10)
        {
            var query = new GetTopFraudTypesQuery { Limit = limit };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("risk-distribution")]
        public async Task<IActionResult> GetRiskDistribution()
        {
            var query = new GetRiskDistributionQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("device-statistics")]
        public async Task<IActionResult> GetDeviceStatistics()
        {
            var query = new GetDeviceStatisticsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("verification-statistics")]
        public async Task<IActionResult> GetVerificationStatistics()
        {
            var query = new GetVerificationStatisticsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}