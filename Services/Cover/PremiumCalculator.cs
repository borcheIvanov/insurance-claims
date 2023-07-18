namespace Services.Cover;

public abstract class PremiumCalculator
{
	private readonly decimal _basePremiumDaily;
	protected abstract decimal Multiplier { get; }
	protected abstract decimal Discount150daysMultiplier { get; }
	protected abstract decimal DiscountRestOfCoverPeriodMultiplier { get; }
	private decimal PremiumPerDay => _basePremiumDaily * Multiplier;

	protected PremiumCalculator(decimal basePremium)
	{
		_basePremiumDaily = basePremium;
	}
	
	public virtual decimal CalculatePremium(DateOnly startDate, DateOnly endDate)
	{
		var first30days = GetInitial30daysPeriod(startDate, endDate);
		var following150days = GetFollowing150daysPeriod(startDate, endDate);
		var restOfPeriod = GetRestPeriod(startDate, endDate);

		return first30days * PremiumPerDay 
		       + following150days * (PremiumPerDay - PremiumPerDay * Discount150daysMultiplier) 
		       + restOfPeriod * (PremiumPerDay - PremiumPerDay * DiscountRestOfCoverPeriodMultiplier);
	}
	
	private static int GetRestPeriod(DateOnly startDate, DateOnly endDate)
	{
		var totalDays = endDate.DayNumber - startDate.DayNumber;
		if (totalDays > 180) {
			return totalDays - 180;
		}
		
		return 0;
	}

	private static int GetFollowing150daysPeriod(DateOnly startDate, DateOnly endDate)
	{
		var totalDays = endDate.DayNumber - startDate.DayNumber;

		return totalDays switch {
			<= 30 => 0,
			> 180 => 150,
			_ => totalDays - 30
		};
	}

	private static int GetInitial30daysPeriod(DateOnly startDate, DateOnly endDate)
	{
		var totalDays = endDate.DayNumber - startDate.DayNumber;
		return totalDays > 30 ? 30 : totalDays;
	}
}