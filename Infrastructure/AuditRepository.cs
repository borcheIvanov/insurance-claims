using Services.Audit;

namespace Infrastructure;

public class AuditRepository: IAuditRepository
{
	private readonly AuditContext _auditContext;

	public AuditRepository(AuditContext auditContext)
	{
		_auditContext = auditContext;
	}
	public async Task AddAsync(AuditBase audit)
	{
		_auditContext.Add(audit);
		await _auditContext.SaveChangesAsync();
	}
}