using Azure;
using Azure.Messaging.EventGrid;
using Microsoft.Extensions.Configuration;
using Services.Audit;

namespace Infrastructure;

public class AuditEventGridRepository: IAuditRepository
{
	private readonly EventGridPublisherClient _publisher;
	
	public AuditEventGridRepository(IConfiguration configuration)
	{
		var endpoint = configuration["EventGrid:Endpoint"] ?? throw new InvalidOperationException(); 
		var key = configuration["EventGrid:Key"] ?? throw new InvalidOperationException();
		
		_publisher = new EventGridPublisherClient(new Uri(endpoint), new AzureKeyCredential(key));
	}
	public async Task AddAsync(AuditBase audit)
	{
		var @event = new EventGridEvent("audit", audit.GetType().Name, "1.0", audit);
		await _publisher.SendEventAsync(@event);
	}
}