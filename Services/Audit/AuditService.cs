namespace Services.Audit;

public class AuditService: IAuditService
{
    private readonly IAuditRepository _auditRepository;

    public AuditService(IAuditRepository auditRepository)
    {
        _auditRepository = auditRepository;
    }

    public async Task AuditClaim(string id, string httpRequestType)
    {
        var claimAudit = new ClaimAudit()
        {
            Created = DateTime.Now,
            HttpRequestType = httpRequestType,
            ClaimId = id
        };

        await _auditRepository.AddAsync(claimAudit);
    }
        
    public async Task AuditCover(string id, string httpRequestType)
    {
        var coverAudit = new CoverAudit()
        {
            Created = DateTime.Now,
            HttpRequestType = httpRequestType,
            CoverId = id
        };

        await _auditRepository.AddAsync(coverAudit);
    }
}