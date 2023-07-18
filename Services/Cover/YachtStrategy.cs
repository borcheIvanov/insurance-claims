namespace Services.Cover;

public class YachtStrategy: PremiumCalculator
{
	public const decimal YachtMultiplier = 1.1M;
	protected override decimal Multiplier => YachtMultiplier;
	protected override decimal Discount150daysMultiplier { get; } = 0.05M;
	protected override decimal DiscountRestOfCoverPeriodMultiplier { get; } = 0.08M;

	public YachtStrategy(decimal basePremium) : base(basePremium) { }
}