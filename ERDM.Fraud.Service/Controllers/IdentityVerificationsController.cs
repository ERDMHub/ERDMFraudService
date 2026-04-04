using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERDM.Fraud.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityVerificationsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<IdentityVerificationsController> _logger;

        public IdentityVerificationsController(IMediator mediator, ILogger<IdentityVerificationsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Commands

        [HttpPost("initiate")]
        public async Task<IActionResult> InitiateVerification([FromBody] InitiateIdentityVerificationCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{verificationId}/complete")]
        public async Task<IActionResult> CompleteVerification(string verificationId, [FromBody] CompleteIdentityVerificationCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.VerificationId = verificationId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{verificationId}/liveness")]
        public async Task<IActionResult> ProcessLiveness(string verificationId, [FromBody] ProcessLivenessDetectionCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.VerificationId = verificationId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("biometric/register")]
        public async Task<IActionResult> RegisterBiometric([FromBody] RegisterBiometricCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("biometric/verify")]
        public async Task<IActionResult> VerifyBiometric([FromBody] VerifyBiometricCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Queries

        [HttpGet("{verificationId}")]
        public async Task<IActionResult> GetVerificationById(string verificationId)
        {
            var query = new GetIdentityVerificationByIdQuery { VerificationId = verificationId };
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetVerificationsByCustomer(string customerId)
        {
            var query = new GetIdentityVerificationsByCustomerQuery { CustomerId = customerId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingVerifications([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var query = new GetPendingVerificationsQuery { PageNumber = page, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        #endregion
    }
}