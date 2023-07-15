namespace Services;

public interface IRepository<T> where T: class, IIdMarker
{
	Task<IEnumerable<T>> GetAllAsync();
	Task<T?> GetByIdAsync(string id);
	Task AddItemAsync(T item);
	Task DeleteItemAsync(string id);
}