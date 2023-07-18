using System.Text;
using System.Text.Json;
using Microsoft.Azure.ServiceBus;

namespace Api.Services;

public class MessagePublisher : IMessagePublisher
{
    private readonly ITopicClient _topicClient;
    
    public MessagePublisher(ITopicClient topicClient)
    {
        _topicClient = topicClient;
    }
    
    public Task Publish<T>(T message) where T : class
    {
        var jsonObj = JsonSerializer.Serialize(message);
        var messageBody = new Message(Encoding.UTF8.GetBytes(jsonObj))
        {
            UserProperties =
            {
                ["message-type"] = typeof(T).Name
            }
        };
        return _topicClient.SendAsync(messageBody);
    }
    
    public Task Publish(string message, CancellationToken cancellationToken)
    {
        var messageBody = new Message(Encoding.UTF8.GetBytes(message));
        messageBody.UserProperties["message-type"] = "RAW";
        return _topicClient.SendAsync(messageBody);
    }
}