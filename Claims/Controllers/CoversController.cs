using Microsoft.AspNetCore.Mvc;
using Services.Audit;
using Services.Cover;

namespace Claims.Controllers;

[ApiController]
[Route("[controller]")]
public class CoversController : ControllerBase
{
    private readonly ILogger<CoversController> _logger;
    private readonly IAuditService _auditService;
    private readonly ICoverService _coverService;

    public CoversController(ICoverService coverService, IAuditService auditService, ILogger<CoversController> logger)
    {
        _coverService = coverService;
        _logger = logger;
        _auditService = auditService;
    }
    
    [HttpPost]
    public IActionResult ComputePremiumAsync(DateOnly startDate, DateOnly endDate, CoverType coverType)
    {
        var premium = _coverService.ComputePremium(startDate, endDate, coverType);
        return Ok(premium);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cover>>> GetAsync()
    {
        var results = await _coverService.GetAllCoversAsync();
        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cover>> GetAsync(string id)
    {
        var result = await _coverService.GetCoverByIdAsync(id);
        if (result is null) {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync(Cover cover)
    {
        cover.Id = Guid.NewGuid().ToString();
        cover.Premium = _coverService.ComputePremium(cover.StartDate, cover.EndDate, cover.Type);
        await _coverService.CreateCoverAsync(cover);
        await _auditService.AuditCover(cover.Id, "POST");
        return Ok(cover);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        await _auditService.AuditCover(id, "DELETE");
        await _coverService.DeleteCoverAsync(id);
        return NoContent();
    }
}