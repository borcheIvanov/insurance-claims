using Services.Claim;

namespace Claims;

/// <summary>
/// Request model for creating Claim
/// </summary>
public class ClaimRequestModel
{
	/// <summary>
	/// Id of the Cover for the Claim we want to create 
	/// </summary>
	public string CoverId { get; set; } = null!;

	/// <summary>
	/// Created Date
	/// </summary>
	public DateTime Created { get; set; }

	/// <summary>
	/// Name for the Claim
	/// </summary>
	public string Name { get; set; } = null!;

	/// <summary>
	/// Claim type
	/// </summary>
	public ClaimType Type { get; set; }

	/// <summary>
	/// Damage cost
	/// </summary>
	public decimal DamageCost { get; set; }
}