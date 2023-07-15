namespace Services.Audit;

public abstract class AuditBase
{
	public int Id { get; set; }

	public DateTime Created { get; set; }

	public string? HttpRequestType { get; set; }
}