using System.Text;
using System.Text.Json;
using Microsoft.Azure.ServiceBus;
using ServiceBus.Contracts;

namespace ServiceBus.Consumer.Services;

public class CustomerConsumerService : BackgroundService
{
    private readonly ISubscriptionClient _subscriptionClient;
    private readonly ILogger<CustomerConsumerService> _logger;

    public CustomerConsumerService(ISubscriptionClient subscriptionClient, ILogger<CustomerConsumerService> logger)
    {
        _subscriptionClient = subscriptionClient;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _subscriptionClient.RegisterMessageHandler(async (message, token) =>
        {
            var weatherForecast = JsonSerializer.Deserialize<CreateWeatherForecast>(Encoding.UTF8.GetString(message.Body));

            _logger.LogInformation("Weather for date {WeatherForecastDate} received", weatherForecast?.Date);
            
            await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }, new MessageHandlerOptions(async args =>
        {
            _logger.LogError("{ArgsException}",args.Exception);
            await Task.CompletedTask;
        })
        {
            MaxConcurrentCalls = 1,
            AutoComplete = false
        });
        return Task.CompletedTask;
    }
}