using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Services.Audit;
using Services.Claim;
using Swashbuckle.AspNetCore.Annotations;

namespace Claims.Controllers;

/// <summary>
/// Controller that contains the endpoints for Claims operations
/// </summary>
[PublicAPI]
[ApiController]
[Route("[controller]")]
public class ClaimsController : ControllerBase
{
        
    private readonly ILogger<ClaimsController> _logger;
    private readonly IClaimService _claimService;
    private readonly IAuditService _auditService;

    /// <summary>
    ///  Constructor with the dependencies required for the endpoints
    /// </summary>
    public ClaimsController(ILogger<ClaimsController> logger, IClaimService claimService, IAuditService auditService)
    {
        _logger = logger;
        _claimService = claimService;
        _auditService = auditService;
    }

    /// <summary>
    /// Get all list of Claims 
    /// </summary>
    [SwaggerResponse(200, "Returns list of Claims")]
    [HttpGet]
    public Task<IEnumerable<Claim>> GetAsync()
    {
        return _claimService.GetClaimsAsync();
    }

    /// <summary>
    /// Create a new Claim
    /// </summary>
    /// <param name="claim">Claim object to be created</param>
    [SwaggerResponse(200, "Returns the Created Claim")]
    [HttpPost]
    public async Task<ActionResult> CreateAsync(Claim claim)
    {
        claim.Id = Guid.NewGuid().ToString();
        await _claimService.AddItemAsync(claim);
        await _auditService.AuditClaim(claim.Id, "POST");
        return Ok(claim);
    }

    /// <summary>
    /// Delete a Claim
    /// </summary>
    /// <param name="id">Id of the desired Claim to be deleted</param>
    [SwaggerResponse(202, "Claim deleted")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await _auditService.AuditClaim(id, "DELETE");
        await _claimService.DeleteItemAsync(id);
        return NoContent();
    }

    /// <summary>
    /// Get Claim by id
    /// </summary>
    /// <param name="id">Id of the desired Claim</param>
    /// <returns>The found Claim or null</returns>
    [HttpGet("{id}")]
    public async Task<Claim?> GetAsync(string id)
    {
        return await _claimService.GetClaimAsync(id);
    }
}