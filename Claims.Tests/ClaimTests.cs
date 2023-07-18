using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Extensions;
using Services.Claim;
using Services.Cover;
using Xunit;

namespace Claims.Tests;

public class ClaimTests
{
	private readonly Cover _cover;
	public ClaimTests()
	{
		_cover = new Cover(DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
							DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1).AddMonths(-1)), 
							CoverType.Tanker);
	}
	
	[Theory]
	[InlineData(100_001)]
	[InlineData(200_000)]
	
	public void Damage_costs_cannot_exceed_100_000(decimal damage)
	{
		Assert.Throws<ArgumentException>(() =>
			new Claim(_cover, DateTime.Now.AddMonths(2), "c", ClaimType.Collision, damage));
	}

	[Fact]
	public void Created_must_be_in_covered_period()
	{
		Assert.Throws<ArgumentException>(() =>
			new Claim(_cover, DateTime.UtcNow.AddMonths(-1), "c", ClaimType.Collision, 10));
	}

	[Fact]
	public void Create_claim_success()
	{
		var claim = new Claim(_cover, DateTime.UtcNow.AddMonths(+3), "testName", ClaimType.Collision, 10);

		using (new AssertionScope()) {
			claim.Should().NotBeNull();
			claim.Name.Should().Be("testName");
			claim.Type.Should().Be(ClaimType.Collision);
			claim.DamageCost.Should().Be(10);
			claim.Created.Should().BeCloseTo(DateTime.UtcNow.AddMonths(+3), 100.Milliseconds());
		}
	}
}