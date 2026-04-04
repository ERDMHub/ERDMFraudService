using MediatR;
using Microsoft.AspNetCore.Mvc;
using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Application.Queries;

namespace ERDM.Fraud.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyntheticIdentityController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SyntheticIdentityController> _logger;

        public SyntheticIdentityController(IMediator mediator, ILogger<SyntheticIdentityController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Commands

        [HttpPost("detect")]
        public async Task<IActionResult> DetectSyntheticIdentity([FromBody] DetectSyntheticIdentityCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateIdentity([FromBody] ValidateIdentityAttributesCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Queries

        [HttpGet("indicators")]
        public async Task<IActionResult> GetIndicators()
        {
            var query = new GetSyntheticIdentityIndicatorsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCustomerSyntheticRisk(string customerId)
        {
            var query = new GetCustomerSyntheticRiskQuery { CustomerId = customerId };
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : NotFound(result);
        }

        #endregion
    }
}