using MediatR;
using Microsoft.AspNetCore.Mvc;
using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Application.Queries;

namespace ERDM.Fraud.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FraudCasesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FraudCasesController> _logger;

        public FraudCasesController(IMediator mediator, ILogger<FraudCasesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Commands

        [HttpPost("open")]
        public async Task<IActionResult> OpenFraudCase([FromBody] OpenFraudCaseCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{caseId}/evidence")]
        public async Task<IActionResult> AddEvidence(string caseId, [FromBody] AddFraudEvidenceCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.CaseId = caseId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("link")]
        public async Task<IActionResult> LinkCases([FromBody] LinkFraudCasesCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{caseId}/assign")]
        public async Task<IActionResult> AssignCase(string caseId, [FromBody] AssignFraudCaseCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.CaseId = caseId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{caseId}/resolve")]
        public async Task<IActionResult> ResolveCase(string caseId, [FromBody] ResolveFraudCaseCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.CaseId = caseId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{caseId}/escalate")]
        public async Task<IActionResult> EscalateCase(string caseId, [FromBody] EscalateFraudCaseCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.CaseId = caseId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Queries

        [HttpGet("{caseId}")]
        public async Task<IActionResult> GetFraudCase(string caseId)
        {
            var query = new GetFraudCaseByIdQuery { CaseId = caseId };
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetFraudCasesByCustomer(string customerId)
        {
            var query = new GetFraudCasesByCustomerQuery { CustomerId = customerId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetFraudCasesByStatus(string status, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var query = new GetFraudCasesByStatusQuery { Status = status, PageNumber = page, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("type/{fraudType}")]
        public async Task<IActionResult> GetFraudCasesByType(string fraudType, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var query = new GetFraudCasesByTypeQuery { FraudType = fraudType, PageNumber = page, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("high-risk")]
        public async Task<IActionResult> GetHighRiskCases([FromQuery] int minRiskScore = 60)
        {
            var query = new GetHighRiskCasesQuery { MinRiskScore = minRiskScore };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("network/{customerId}")]
        public async Task<IActionResult> GetFraudNetwork(string customerId, [FromQuery] int depth = 3)
        {
            var query = new GetFraudNetworkQuery { CustomerId = customerId, Depth = depth };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        #endregion
    }
}