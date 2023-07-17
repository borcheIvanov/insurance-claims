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
		return new Cover(startDate, endDate, coverType).Premium;
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