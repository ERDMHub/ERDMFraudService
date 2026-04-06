using ERDM.Fraud.Service.Application.Commands;
using ERDM.Fraud.Service.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace ERDM.Fraud.Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FraudAlertsController : ControllerBase
    {
        private readonly IFraudAlertService _service;
        private readonly ILogger<FraudAlertsController> _logger;

        public FraudAlertsController(IFraudAlertService service, ILogger<FraudAlertsController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // POST: api/fraudalerts
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFraudAlertCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateAsync(command);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // PUT: api/fraudalerts/{alertId}/resolve
        [HttpPut("{alertId}/resolve")]
        public async Task<IActionResult> Resolve(string alertId)
        {
            var result = await _service.ResolveAsync(alertId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // DELETE: api/fraudalerts/{alertId}
        [HttpDelete("{alertId}")]
        public async Task<IActionResult> Delete(string alertId)
        {
            var result = await _service.DeleteAsync(alertId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // GET: api/fraudalerts/{alertId}
        [HttpGet("{alertId}")]
        public async Task<IActionResult> GetById(string alertId)
        {
            var result = await _service.GetByIdAsync(alertId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        // GET: api/fraudalerts/customer/{customerId}
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomer(string customerId)
        {
            var result = await _service.GetByCustomerIdAsync(customerId);
            return Ok(result);
        }

        // GET: api/fraudalerts/unresolved
        [HttpGet("unresolved")]
        public async Task<IActionResult> GetUnresolved([FromQuery] string? severity = null)
        {
            var result = await _service.GetUnresolvedAsync(severity);
            return Ok(result);
        }

        // GET: api/fraudalerts
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }
    }
}
