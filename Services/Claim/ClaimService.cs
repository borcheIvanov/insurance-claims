namespace Services.Claim;

public class ClaimService : IClaimService
{
	private readonly IRepository<Claim> _repository;
	public ClaimService(IRepository<Claim> repository)
	{
		_repository = repository;
	}

	public async Task<IEnumerable<Claim>> GetClaimsAsync()
	{
		return await _repository.GetAllAsync();
	}

	public async Task<Claim?> GetClaimAsync(string id)
	{
		return await _repository.GetByIdAsync(id);
	}

	public Task AddItemAsync(Claim item)
	{
		return _repository.AddItemAsync(item);
	}

	public Task DeleteItemAsync(string id)
	{
		return _repository.DeleteItemAsync(id);
	}
}