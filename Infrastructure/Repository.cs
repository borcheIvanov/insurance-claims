using Microsoft.Azure.Cosmos;
using Services;

namespace Infrastructure;

public class Repository<T>: IRepository<T> where T : class, IIdMarker
{
	private readonly Container _container;
	
	public Repository(CosmosClient dbClient,
		string databaseName,
		string containerName)
	{
		if (dbClient == null) throw new ArgumentNullException(nameof(dbClient));
		_container = dbClient.GetContainer(databaseName, containerName);
	}
	
	public async Task<IEnumerable<T>> GetAllAsync()
	{
		var query = _container.GetItemQueryIterator<T>(new QueryDefinition("SELECT * FROM c"));
		var results = new List<T>();
		while (query.HasMoreResults)
		{
			var response = await query.ReadNextAsync();

			results.AddRange(response.ToList());
		}
		return results;
	}

	public async Task<T?> GetByIdAsync(string id)
	{
		try
		{
			var response = await _container.ReadItemAsync<T>(id, new PartitionKey(id));
			return response.Resource;
		}
		catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
		{
			return null;
		}
	}

	public async Task AddItemAsync(T item)
	{
		await _container.CreateItemAsync(item, new PartitionKey(item.Id));
	}

	public Task DeleteItemAsync(string id)
	{
		return _container.DeleteItemAsync<T>(id, new PartitionKey(id));
	}
}