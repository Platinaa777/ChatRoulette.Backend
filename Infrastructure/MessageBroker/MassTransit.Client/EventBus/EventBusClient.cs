namespace MassTransit.Client.EventBus;

public class EventBusClient : IEventBusClient
{
    private readonly IPublishEndpoint _publisher;

    public EventBusClient(IPublishEndpoint publisher)
    {
        _publisher = publisher;
    }
    
    public Task PublishAsync<T>(T message, CancellationToken token) where T : class
    {
        return _publisher.Publish<T>(message, token);
    }
}