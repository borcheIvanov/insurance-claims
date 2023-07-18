using FluentValidation;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Services.Audit;
using Services.Cover;
using Swashbuckle.AspNetCore.Annotations;

namespace Claims.Controllers;

/// <summary>
/// Controller that contains the endpoints for Covers operations
/// </summary>
[PublicAPI]
[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ILogger<CoversController> _logger;
    private readonly IValidator<CoverRequestModel> _validator;
    private readonly IAuditService _auditService;
    private readonly ICoverService _coverService;

    /// <summary>
    /// Constructor with the required dependency by dependency injection
    /// </summary>
    public CoversController(ICoverService coverService, IAuditService auditService, ILogger<CoversController> logger,
        IValidator<CoverRequestModel> validator)
    {
        _coverService = coverService;
        _logger = logger;
        _validator = validator;
        _auditService = auditService;
    }

    /// <summary>
    ///  Get a premium calculation with the desired query parameters 
    /// </summary>
    /// <param name="coverRequest"></param>
    [SwaggerResponse(200, "Result of the computation for the passed parameters", typeof(decimal))]
    [SwaggerResponse(400, "Bad request containing list of errors when the Request is invalid")]
    [HttpPost("compute-premium")]
    public async Task<IActionResult> ComputePremiumAsync([FromQuery] CoverRequestModel coverRequest)
    {
        var validationResult = await _validator.ValidateAsync(coverRequest);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors.ToArray());
        }
        
        var premium = _coverService.ComputePremium(coverRequest.StartDate, coverRequest.EndDate, coverRequest.CoverType);
        return Ok(premium);
    }

    /// <summary>
    /// Get a lis of all existing Covers
    /// </summary>
    [SwaggerResponse(200, "List of all Covers", typeof(IEnumerable<Cover>))]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cover>>> GetAsync()
    {
        var results = await _coverService.GetAllCoversAsync();
        return Ok(results);
    }

    /// <summary>
    /// Get Cover by its id
    /// </summary>
    /// <param name="id">Id of the desired Cover</param>
    [SwaggerResponse(404, "Cover with that id is not found")]
    [SwaggerResponse(200, "Cover with the passed Id", typeof(Cover))]
    [HttpGet("{id}")]
    public async Task<ActionResult<Cover>> GetAsync(string id)
    {
        var result = await _coverService.GetCoverByIdAsync(id);
        if (result is null) {
            return NotFound();
        }

        return Ok(result);
    }

    /// <summary>
    /// Create a new Cover
    /// </summary>
    /// <param name="createCoverRequest">Cover to be created</param>
    [SwaggerResponse(200, "Returns the created Cover", typeof(Cover))] 
    [SwaggerResponse(400, "Bad request containing list of errors when the Request is invalid")]
    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody]CoverRequestModel createCoverRequest)
    {
        var validationResult = await _validator.ValidateAsync(createCoverRequest);
        if (!validationResult.IsValid) {
            return BadRequest(validationResult.Errors.ToArray());
        }

        var cover = new Cover(createCoverRequest.StartDate, createCoverRequest.EndDate, createCoverRequest.CoverType);
        await _coverService.CreateCoverAsync(cover);

        await _auditService.AuditCover(cover.Id, "POST");
        return Ok(cover);
    }

    /// <summary>
    /// Delete a Cover
    /// </summary>
    /// <param name="id">Id of the Cover to be deleted</param>
    [SwaggerResponse(202, "Cover Deleted")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await _auditService.AuditCover(id, "DELETE");
        await _coverService.DeleteCoverAsync(id);
        return NoContent();
    }
}