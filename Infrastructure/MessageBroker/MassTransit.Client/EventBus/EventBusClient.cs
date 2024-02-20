namespace MassTransit.Client.EventBus;

public class EventBusClient : IEventBusClient
{
    private readonly IBus _bus;
    private readonly IPublishEndpoint _publisher;

    public EventBusClient(IBus bus, IPublishEndpoint publisher)
    {
        _bus = bus;
        _publisher = publisher;
    }
    
    public Task PublishAsync<T>(T message, CancellationToken token) where T : class
    {
        return _publisher.Publish<T>(message, token);
    }
}