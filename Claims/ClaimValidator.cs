using FluentValidation;
using Services.Claim;
using Services.Cover;

namespace Claims;

internal class ClaimValidator: AbstractValidator<ClaimRequestModel>
{
	private readonly ICoverService _coverService;

	public ClaimValidator(ICoverService coverService)
	{
		_coverService = coverService;

		RuleFor(c => c)
			.Cascade(CascadeMode.Stop)
			.MustAsync(async (c, _) => await CoverExists(c.CoverId))
			.WithMessage("Cover does not exists")
			.MustAsync(async (c, _) => await CreatedIsInCoverPeriod(c.CoverId, c.Created))
			.WithMessage("Created Date must be in Cover Period");

		RuleFor(c => c.DamageCost)
			.LessThanOrEqualTo(Claim.MaximumDamageAllowed)
			.WithMessage("DamageCost cannot exceed 100.000");
	}

	private async Task<bool> CoverExists(string coverId)
	{
		var cover = await _coverService.GetCoverByIdAsync(coverId);
		return cover is not null;
	}
	
	private async Task<bool> CreatedIsInCoverPeriod(string coverId, DateTime created)
	{
		var cover = await _coverService.GetCoverByIdAsync(coverId);
		return Claim.CreatedIsInCoverPeriod(cover!, created);
	}
}