using JetBrains.Annotations;
using Services.Cover;

namespace Claims;

/// <summary>
/// Request model for creating new Cover
/// </summary>
[PublicAPI]
public class CoverRequestModel
{
	/// <summary>
	/// Start date for the insurance period
	/// </summary>
	public DateOnly StartDate { get; set; }
	/// <summary>
	/// End date for the insurance period 
	/// </summary>
	public DateOnly EndDate { get; set; }
	/// <summary>
	/// Cover Type
	/// </summary>
	public CoverType CoverType { get; set; }
}