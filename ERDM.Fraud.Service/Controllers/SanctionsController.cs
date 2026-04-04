using MediatR;
using Microsoft.AspNetCore.Mvc;
using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Application.Queries;

namespace ERDM.Fraud.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SanctionsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SanctionsController> _logger;

        public SanctionsController(IMediator mediator, ILogger<SanctionsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Commands

        [HttpPost("screen")]
        public async Task<IActionResult> ScreenCustomer([FromBody] ScreenCustomerCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("screen/batch")]
        public async Task<IActionResult> ScreenBatch([FromBody] ScreenBatchCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Queries

        [HttpGet("history/{customerId}")]
        public async Task<IActionResult> GetScreeningHistory(string customerId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var query = new GetSanctionsScreeningHistoryQuery { CustomerId = customerId, PageNumber = page, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("hits")]
        public async Task<IActionResult> GetSanctionsHits([FromQuery] string? customerId, [FromQuery] string? listName)
        {
            var query = new GetSanctionsHitsQuery { CustomerId = customerId, ListName = listName };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("lists")]
        public async Task<IActionResult> GetAvailableLists()
        {
            var query = new GetAvailableSanctionsListsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        #endregion
    }
}