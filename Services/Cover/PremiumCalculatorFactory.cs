namespace Services.Cover;

public static class PremiumCalculatorFactory
{
	public static PremiumCalculator Create(CoverType type, decimal basePremium)
	{
		return type switch {
			CoverType.Yacht => new YachtStrategy(basePremium),
			CoverType.Tanker => new TankerStrategy(basePremium),
			CoverType.PassengerShip => new PassengerShipStrategy(basePremium),
			CoverType.BulkCarrier => new BulkCarrierStrategy(basePremium),
			CoverType.ContainerShip => new ContainerShipStrategy(basePremium),
			_ => throw new Exception("")
		};
	}
}