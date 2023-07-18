namespace Services.Cover;

public class PassengerShipStrategy: PremiumCalculator
{
	public const decimal PassengerShipMultiplier = 1.2M;
	public PassengerShipStrategy(decimal basePremium) : base(basePremium) { }
	protected override decimal Multiplier { get; } = PassengerShipMultiplier;
	protected override decimal Discount150daysMultiplier { get; } = 0.02M;
	protected override decimal DiscountRestOfCoverPeriodMultiplier { get; } = 0.03M;
}