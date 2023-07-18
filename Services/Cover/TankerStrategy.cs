namespace Services.Cover;

public class TankerStrategy : PremiumCalculator
{
	public const decimal TankerMultiplier = 1.5M; 
	public TankerStrategy(decimal basePremium) : base(basePremium) { }
	
	protected override decimal Multiplier { get; } = TankerMultiplier;
	protected override decimal Discount150daysMultiplier { get; } = 0.02M;
	protected override decimal DiscountRestOfCoverPeriodMultiplier { get; } = 0.03M;
}