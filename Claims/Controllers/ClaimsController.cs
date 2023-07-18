using FluentValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Services.Audit;
using Services.Claim;
using Services.Cover;
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
    private readonly IValidator<ClaimRequestModel> _claimValidator;
    private readonly ICoverService _coverService;

    /// <summary>
    ///  Constructor with the dependencies required for the endpoints
    /// </summary>
    public ClaimsController(ILogger<ClaimsController> logger, IClaimService claimService, IAuditService auditService,
        IValidator<ClaimRequestModel> claimValidator, ICoverService coverService)
    {
        _logger = logger;
        _claimService = claimService;
        _auditService = auditService;
        _claimValidator = claimValidator;
        _coverService = coverService;
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
    /// <param name="claimRequest">Claim request model to create Claim</param>
    [SwaggerResponse(200, "Returns the Created Claim")]
    [HttpPost]
    public async Task<ActionResult> CreateAsync(ClaimRequestModel claimRequest)
    {
        var validationResult = await _claimValidator.ValidateAsync(claimRequest);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors.ToArray());
        }

        var cover = await _coverService.GetCoverByIdAsync(claimRequest.CoverId);
        var claim = new Claim(cover!, claimRequest.Created, claimRequest.Name, claimRequest.Type,
            claimRequest.DamageCost);
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