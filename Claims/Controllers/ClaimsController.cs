using Microsoft.AspNetCore.Mvc;
using Services.Audit;
using Services.Claim;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class ClaimsController : ControllerBase
{
        
    private readonly ILogger<ClaimsController> _logger;
    private readonly IClaimService _claimService;
    private readonly IAuditService _auditService;

    public ClaimsController(ILogger<ClaimsController> logger, IClaimService claimService, IAuditService auditService)
    {
        _logger = logger;
        _claimService = claimService;
        _auditService = auditService;
    }

    [HttpGet]
    public Task<IEnumerable<Claim>> GetAsync()
    {
        return _claimService.GetClaimsAsync();
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Claim claim)
    {
        claim.Id = Guid.NewGuid().ToString();
        await _claimService.AddItemAsync(claim);
        await _auditService.AuditClaim(claim.Id, "POST");
        return Ok(claim);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await _auditService.AuditClaim(id, "DELETE");
        await _claimService.DeleteItemAsync(id);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<Claim?> GetAsync(string id)
    {
        return await _claimService.GetClaimAsync(id);
    }
}