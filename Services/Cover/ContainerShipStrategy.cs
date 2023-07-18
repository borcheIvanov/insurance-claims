namespace Services.Cover;

public class ContainerShipStrategy : PremiumCalculator
{
	public ContainerShipStrategy(decimal basePremium) : base(basePremium) { }
	protected override decimal Multiplier { get; } = 1.3M;
	protected override decimal Discount150daysMultiplier { get; } = 0.02M;
	protected override decimal DiscountRestOfCoverPeriodMultiplier { get; } = 0.03M;
}