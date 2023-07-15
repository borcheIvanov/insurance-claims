namespace Services.Audit;

public interface IAuditRepository
{
	Task AddAsync(AuditBase audit);
}