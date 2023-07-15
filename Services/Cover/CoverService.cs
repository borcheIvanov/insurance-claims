namespace Services.Cover;

public class CoverService: ICoverService
{
	private readonly IRepository<Cover> _repository;

	public CoverService(IRepository<Cover> repository)
	{
		_repository = repository;
	}
	
	public decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType)
	{
		var multiplier = 1.3m;
		if (coverType == CoverType.Yacht)
		{
			multiplier = 1.1m;
		}

		if (coverType == CoverType.PassengerShip)
		{
			multiplier = 1.2m;
		}

		if (coverType == CoverType.Tanker)
		{
			multiplier = 1.5m;
		}

		var premiumPerDay = 1250 * multiplier;
		var insuranceLength = endDate.DayNumber - startDate.DayNumber;
		var totalPremium = 0m;

		for (var i = 0; i < insuranceLength; i++)
		{
			if (i < 30) totalPremium += premiumPerDay;
			if (i < 180 && coverType == CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.05m;
			else if (i < 180) totalPremium += premiumPerDay - premiumPerDay * 0.02m;
			if (i < 365 && coverType != CoverType.Yacht) totalPremium += premiumPerDay - premiumPerDay * 0.03m;
			else if (i < 365) totalPremium += premiumPerDay - premiumPerDay * 0.08m;
		}

		return totalPremium;
	}

	public async Task<IEnumerable<Cover>> GetAllCoversAsync()
	{ 
		return  await _repository.GetAllAsync();
	}

	public async Task<Cover?> GetCoverByIdAsync(string id)
	{
		return await _repository.GetByIdAsync(id);
	}

	public async Task CreateCoverAsync(Cover cover)
	{
		await _repository.AddItemAsync(cover);
	}

	public async Task DeleteCoverAsync(string id)
	{
		await _repository.DeleteItemAsync(id);
	}
}