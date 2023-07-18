namespace Api.Services;

public interface IMessagePublisher
{
    public Task Publish<T>(T message) where T : class;
    public Task Publish(string message, CancellationToken cancellationToken);
}