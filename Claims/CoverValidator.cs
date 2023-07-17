using FluentValidation;
using Services.Cover;

namespace Claims;

internal class CoverValidator: AbstractValidator<CoverRequestModel>
{
	public CoverValidator()
	{
		RuleFor(c => c.StartDate)
			.Must(Cover.DateIsNotInPast)
			.WithMessage("StartDate cannot be in the past.");

		RuleFor(c => c)
			.Must(c =>  Cover.PeriodIsUnderYear(c.StartDate, c.EndDate))
			.WithMessage("Total insurance period cannot exceed 1 year");
	}
}