using FluentAssertions;
using Services.Cover;
using Xunit;

namespace Claims.Tests;

public class PremiumCalculationTests
{
	// Base day rate was set to be 1250.
	// Yacht should be 10% more expensive, Passenger ship 20%, Tanker 50%, and other types 30%
	// The length of the insurance period should influence the premium progressively:
	//
	// First 30 days are computed based on the logic above
	// Following 150 days are discounted by 5% for Yacht and by 2% for other types
	// The remaining days are discounted by additional 3% for Yacht and by 1% for other types
	
	[Fact]
	public void Yacht_is_10_percent_more_expensive_for_initial_period()
	{
		var expectedPremium = 29 * Cover.DailyPremiumBase * YachtStrategy.YachtMultiplier;  
		
		var cover = new Cover(Date(0), Date(29), CoverType.Yacht);
		
		cover.Premium.Should().Be(expectedPremium);
	}

	[Fact]
	public void Yacht_is_discounted_by_5_following_150_days()
	{
		var premiumForDays30 = 30 * Cover.DailyPremiumBase * YachtStrategy.YachtMultiplier;
		var premiumForDays60 = 60 * (Cover.DailyPremiumBase * YachtStrategy.YachtMultiplier 
		                             - Cover.DailyPremiumBase * YachtStrategy.YachtMultiplier * 0.05M);
		
		var cover = new Cover(Date(0), Date(90), CoverType.Yacht);

		cover.Premium.Should().Be(premiumForDays30 + premiumForDays60);
	}
	
	[Fact]
	public void Yacht_is_further_discounted_by_3_the_rest_of_days()
	{
		var premiumForDays30 = 30 * Cover.DailyPremiumBase * YachtStrategy.YachtMultiplier;
		var premiumForDays150 = 150 * (Cover.DailyPremiumBase * YachtStrategy.YachtMultiplier 
		                               - Cover.DailyPremiumBase * YachtStrategy.YachtMultiplier * 0.05M);
		var premiumForDays180 = 180 * (Cover.DailyPremiumBase * YachtStrategy.YachtMultiplier 
		                               - Cover.DailyPremiumBase * YachtStrategy.YachtMultiplier * 0.08M);
		
		var cover = new Cover(Date(0), Date(360), CoverType.Yacht);

		cover.Premium.Should().Be(premiumForDays30 + premiumForDays150 + premiumForDays180);
	}
	
	[Fact]
	public void PassengerShip_is_20_percent_more_expensive_for_initial_period()
	{
		var expectedPremium = 14 * Cover.DailyPremiumBase * PassengerShipStrategy.PassengerShipMultiplier;  
		
		var cover = new Cover(Date(0), Date(14), CoverType.PassengerShip);
		
		cover.Premium.Should().Be(expectedPremium);
	}
	
	[Fact]
	public void PassengerShip_is_discounted_by_2_following_150_days()
	{
		var expectedPremium = 30 * Cover.DailyPremiumBase * PassengerShipStrategy.PassengerShipMultiplier 
		                      + 100 * (Cover.DailyPremiumBase * PassengerShipStrategy.PassengerShipMultiplier 
		                               - Cover.DailyPremiumBase * PassengerShipStrategy.PassengerShipMultiplier * 0.02M);  
		
		var cover = new Cover(Date(0), Date(130), CoverType.PassengerShip);
		
		cover.Premium.Should().Be(expectedPremium);
	}
	
	[Fact]
	public void PassengerShip_is_further_discounted_by_1_after_180_days()
	{
		var expectedPremium = 30 * Cover.DailyPremiumBase * PassengerShipStrategy.PassengerShipMultiplier
		                      + 150 * (Cover.DailyPremiumBase * PassengerShipStrategy.PassengerShipMultiplier
		                               - Cover.DailyPremiumBase * PassengerShipStrategy.PassengerShipMultiplier * 0.02M)
		                      + 40 * (Cover.DailyPremiumBase * PassengerShipStrategy.PassengerShipMultiplier
		                              - Cover.DailyPremiumBase * PassengerShipStrategy.PassengerShipMultiplier * 0.03M);
		
		var cover = new Cover(Date(0), Date(220), CoverType.PassengerShip);
		
		cover.Premium.Should().Be(expectedPremium);
	}
	
	[Fact]
	public void Tanker_is_50_percent_more_expensive_for_initial_period()
	{
		var expectedPremium = 20 * Cover.DailyPremiumBase * TankerStrategy.TankerMultiplier;  
		
		var cover = new Cover(Date(0), Date(20), CoverType.Tanker);
		
		cover.Premium.Should().Be(expectedPremium);
	}
	
	[Fact]
	public void Tanker_is_discounted_by_2_following_150_days()
	{
		var expectedPremium = 30 * Cover.DailyPremiumBase * TankerStrategy.TankerMultiplier 
		                      + 100 * (Cover.DailyPremiumBase * TankerStrategy.TankerMultiplier 
		                               - Cover.DailyPremiumBase * TankerStrategy.TankerMultiplier * 0.02M);  
		
		var cover = new Cover(Date(0), Date(130), CoverType.Tanker);
		
		cover.Premium.Should().Be(expectedPremium);
	}
	
	[Fact]
	public void Tanker_is_further_discounted_by_1_after_180_days()
	{
		var expectedPremium = 30 * Cover.DailyPremiumBase * TankerStrategy.TankerMultiplier
		                      + 150 * (Cover.DailyPremiumBase * TankerStrategy.TankerMultiplier
		                               - Cover.DailyPremiumBase * TankerStrategy.TankerMultiplier * 0.02M)
		                      + 40 * (Cover.DailyPremiumBase * TankerStrategy.TankerMultiplier
		                              - Cover.DailyPremiumBase * TankerStrategy.TankerMultiplier * 0.03M);
		
		
		var cover = new Cover(Date(0), Date(220), CoverType.Tanker);
		
		cover.Premium.Should().Be(expectedPremium);
	}
	
	[Theory]
	[InlineData(CoverType.BulkCarrier)]
	[InlineData(CoverType.ContainerShip)]
	public void CoverType_is_30_percent_More_expensive(CoverType type)
	{
		var expectedPremium = 20 * Cover.DailyPremiumBase * 1.3M;  
		
		var cover = new Cover(Date(0), Date(20), type);
		
		cover.Premium.Should().Be(expectedPremium);
	}
	
	[Theory]
	[InlineData(CoverType.BulkCarrier)]
	[InlineData(CoverType.ContainerShip)]
	public void Bulk_and_container_is_discounted_by_2_following_150_days(CoverType type)
	{
		var premiumForDays30 = 30 * Cover.DailyPremiumBase * 1.3M;
		var premiumForDays60 = 60 * (Cover.DailyPremiumBase * 1.3M 
		                             - Cover.DailyPremiumBase * 1.3M * 0.02M);
		
		var cover = new Cover(Date(0), Date(90), type);

		cover.Premium.Should().Be(premiumForDays30 + premiumForDays60);
	}
	
	[Theory]
	[InlineData(CoverType.BulkCarrier)]
	[InlineData(CoverType.ContainerShip)]
	public void Bulk_and_container_is_further_discounted_by_1_after_180_days(CoverType type)
	{
		var premiumForDays30 = 30 * Cover.DailyPremiumBase * 1.3M;
		var premiumForDays60 = 150 * (Cover.DailyPremiumBase * 1.3M 
		                             - Cover.DailyPremiumBase * 1.3M * 0.02M);
		var premiumForRest = 20 * (Cover.DailyPremiumBase * 1.3M 
		                           - Cover.DailyPremiumBase * 1.3M * 0.03M);
		
		var cover = new Cover(Date(0), Date(200), type);

		cover.Premium.Should().Be(premiumForDays30 + premiumForDays60 + premiumForRest);
	}

	private DateOnly Date(int days)
	{
		return DateOnly.FromDateTime(DateTime.UtcNow).AddDays(days);
	}
}