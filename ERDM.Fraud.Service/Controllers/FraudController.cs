using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ERDM.Fraud.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FraudController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<FraudController> _logger;

        public FraudController(IMediator mediator, ILogger<FraudController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        #region Device Fingerprinting

        [HttpPost("devices/register")]
        public async Task<IActionResult> RegisterDevice([FromBody] RegisterDeviceCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("devices/{deviceId}/associate")]
        public async Task<IActionResult> AssociateDevice(string deviceId, [FromQuery] string customerId)
        {
            var command = new AssociateDeviceWithCustomerCommand { DeviceId = deviceId, CustomerId = customerId };
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("devices/{deviceId}")]
        public async Task<IActionResult> GetDeviceById(string deviceId)
        {
            var query = new GetDeviceByIdQuery { DeviceId = deviceId };
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("devices/fingerprint/{fingerprintHash}")]
        public async Task<IActionResult> GetDeviceByFingerprint(string fingerprintHash)
        {
            var query = new GetDeviceByFingerprintQuery { FingerprintHash = fingerprintHash };
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("devices/customer/{customerId}")]
        public async Task<IActionResult> GetDevicesByCustomer(string customerId)
        {
            var query = new GetDevicesByCustomerQuery { CustomerId = customerId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("devices/blacklisted")]
        public async Task<IActionResult> GetBlacklistedDevices([FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            var query = new GetBlacklistedDevicesQuery { PageNumber = page, PageSize = size };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("devices/{deviceId}/blacklist")]
        public async Task<IActionResult> BlacklistDevice(string deviceId, [FromBody] BlacklistDeviceCommand command)
        {
            command.DeviceId = deviceId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("devices/{deviceId}/behavioral-pattern")]
        public async Task<IActionResult> AddBehavioralPattern(string deviceId, [FromBody] AddBehavioralPatternCommand command)
        {
            command.DeviceId = deviceId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Identity Verification

        [HttpPost("verification/initiate")]
        public async Task<IActionResult> InitiateVerification([FromBody] InitiateIdentityVerificationCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("verification/{verificationId}/complete")]
        public async Task<IActionResult> CompleteVerification(string verificationId, [FromBody] CompleteIdentityVerificationCommand command)
        {
            command.VerificationId = verificationId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("verification/{verificationId}/liveness")]
        public async Task<IActionResult> ProcessLiveness(string verificationId, [FromBody] ProcessLivenessDetectionCommand command)
        {
            command.VerificationId = verificationId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("biometric/register")]
        public async Task<IActionResult> RegisterBiometric([FromBody] RegisterBiometricCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Fraud Cases

        [HttpPost("cases/open")]
        public async Task<IActionResult> OpenFraudCase([FromBody] OpenFraudCaseCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("cases/{caseId}/evidence")]
        public async Task<IActionResult> AddEvidence(string caseId, [FromBody] AddFraudEvidenceCommand command)
        {
            command.CaseId = caseId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("cases/link")]
        public async Task<IActionResult> LinkCases([FromBody] LinkFraudCasesCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("cases/{caseId}/resolve")]
        public async Task<IActionResult> ResolveCase(string caseId, [FromBody] ResolveFraudCaseCommand command)
        {
            command.CaseId = caseId;
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("cases/{caseId}")]
        public async Task<IActionResult> GetFraudCase(string caseId)
        {
            var query = new GetFraudCaseByIdQuery { CaseId = caseId };
            var result = await _mediator.Send(query);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("cases/customer/{customerId}")]
        public async Task<IActionResult> GetFraudCasesByCustomer(string customerId)
        {
            var query = new GetFraudCasesByCustomerQuery { CustomerId = customerId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("cases/status/{status}")]
        public async Task<IActionResult> GetFraudCasesByStatus(string status, [FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            var query = new GetFraudCasesByStatusQuery { Status = status, PageNumber = page, PageSize = size };
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

        #region Sanctions Screening

        [HttpPost("sanctions/screen")]
        public async Task<IActionResult> ScreenCustomer([FromBody] ScreenCustomerCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("sanctions/history/{customerId}")]
        public async Task<IActionResult> GetSanctionsHistory(string customerId, [FromQuery] int page = 1, [FromQuery] int size = 20)
        {
            var query = new GetSanctionsScreeningHistoryQuery { CustomerId = customerId, PageNumber = page, PageSize = size };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("sanctions/hits")]
        public async Task<IActionResult> GetSanctionsHits([FromQuery] string? customerId, [FromQuery] string? listName)
        {
            var query = new GetSanctionsHitsQuery { CustomerId = customerId, ListName = listName };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        #endregion

        #region Synthetic Identity Detection

        [HttpPost("synthetic/detect")]
        public async Task<IActionResult> DetectSyntheticIdentity([FromBody] DetectSyntheticIdentityCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region Statistics

        [HttpGet("statistics")]
        public async Task<IActionResult> GetStatistics([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            var query = new GetFraudStatisticsQuery { FromDate = fromDate, ToDate = toDate };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        #endregion
    }
}
