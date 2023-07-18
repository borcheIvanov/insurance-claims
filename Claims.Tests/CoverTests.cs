using FluentAssertions;
using FluentAssertions.Execution;
using Services.Cover;
using Xunit;

namespace Claims.Tests;

public class CoverTests
{
	[Fact]
	public void Start_date_cannot_be_in_the_past()
	{
		Assert.Throws<ArgumentException>(() =>
			new Cover(DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)), 
					   DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)), 
					   CoverType.Tanker));
	}
	
	[Fact]
	public void Insurance_period_cannot_exceed_1_year()
	{
		Assert.Throws<ArgumentException>(() =>
			new Cover(DateOnly.FromDateTime(DateTime.UtcNow), 
				DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1).AddDays(1)), 
				CoverType.Tanker));
	}

	[Fact]
	public void Create_Cover_success()
	{
		var cover = new Cover(DateOnly.FromDateTime(DateTime.UtcNow), 
							   DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)), 
							   CoverType.Tanker);

		using (new AssertionScope()) {
			cover.Should().NotBeNull();
			cover.Type.Should().Be(CoverType.Tanker);
			cover.StartDate.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow));
			cover.EndDate.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(1)));
		}
	}
}