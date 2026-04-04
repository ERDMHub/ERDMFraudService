using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Application.Commands.DeviceCommands;
using ERDM.Fraud.Service.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERDM.Fraud.Service.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceFingerprintsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DeviceFingerprintsController> _logger;

        public DeviceFingerprintsController(IMediator mediator, ILogger<DeviceFingerprintsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Commands

        [HttpPost("register")]
        public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{deviceId}/associate")]
        public async Task<IActionResult> AssociateDevice(string deviceId, [FromQuery] string customerId)
        {
            var command = new AssociateDeviceWithCustomerCommand
            {
                DeviceId = deviceId,
                CustomerId = customerId
            };
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{deviceId}/risk-score")]
        public async Task<IActionResult> UpdateRiskScore(string deviceId, [FromBody] UpdateDeviceRiskScoreCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.DeviceId = deviceId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{deviceId}/blacklist")]
        public async Task<IActionResult> BlacklistDevice(string deviceId, [FromBody] BlacklistDeviceCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.DeviceId = deviceId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("{deviceId}/behavioral-pattern")]
        public async Task<IActionResult> AddBehavioralPattern(string deviceId, [FromBody] AddBehavioralPatternCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            command.DeviceId = deviceId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Queries

        [HttpGet("{deviceId}")]
        public async Task<IActionResult> GetDeviceById(string deviceId)
        {
            var query = new GetDeviceByIdQuery { DeviceId = deviceId };
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("fingerprint/{fingerprintHash}")]
        public async Task<IActionResult> GetDeviceByFingerprint(string fingerprintHash)
        {
            var query = new GetDeviceByFingerprintQuery { FingerprintHash = fingerprintHash };
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetDevicesByCustomer(string customerId)
        {
            var query = new GetDevicesByCustomerQuery { CustomerId = customerId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("blacklisted")]
        public async Task<IActionResult> GetBlacklistedDevices([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var query = new GetBlacklistedDevicesQuery { PageNumber = page, PageSize = pageSize };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("high-risk")]
        public async Task<IActionResult> GetHighRiskDevices([FromQuery] int minRiskScore = 60, [FromQuery] int limit = 100)
        {
            var query = new GetHighRiskDevicesQuery { MinRiskScore = minRiskScore, Take = limit };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        #endregion
    }
}