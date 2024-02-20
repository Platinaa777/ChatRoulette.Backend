namespace MassTransit.Client.EventBus;

public interface IEventBusClient
{
    Task PublishAsync<T>(T message, CancellationToken token) where T : class;
}