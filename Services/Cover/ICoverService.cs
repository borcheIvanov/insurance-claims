namespace Services.Cover;

public interface ICoverService
{
	decimal ComputePremium(DateOnly startDate, DateOnly endDate, CoverType coverType);
	Task<IEnumerable<Cover>> GetAllCoversAsync();
	Task<Cover?> GetCoverByIdAsync(string id);

	Task CreateCoverAsync(Cover cover);
	Task DeleteCoverAsync(string id);
}