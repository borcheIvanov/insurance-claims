namespace Services.Audit;

public interface IAuditService
{
	/// <summary>
	/// Adds new audit log of type Claim
	/// </summary>
	/// <param name="id">Id of the Claim that is acted on</param>
	/// <param name="httpRequestType">Request type</param>
	/// <returns></returns>
	Task AuditClaim(string id, string httpRequestType);

	/// <summary>
	/// Adds new audit log of type Cover
	/// </summary>
	/// <param name="id">Id of the Cover that is acted on</param>
	/// <param name="httpRequestType">Request type</param>
	Task AuditCover(string id, string httpRequestType);
}