namespace Claims;

internal class CosmosDbConfiguration
{
	public string DatabaseName { get; set; } = null!;
	public string ContainerName { get; set; } = null!;
	public string Account { get; set; } = null!;
	public string Key { get; set; } = null!;
}