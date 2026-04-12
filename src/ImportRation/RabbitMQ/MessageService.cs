using System.Text;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ImportRation.RabbitMQ;

public class MessageService : IDisposable, IRabbitMQPublisher
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ImportRationMessageBroker _settings;

    public MessageService(IOptions<ImportRationMessageBroker> settings)
    {
        _settings = settings.Value;

        var factory = new ConnectionFactory()
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password
        };

        // Async connection and channel creation
        _connection =
            factory.CreateConnection(); // No need for async here, unless you're using advanced connection handling
        _channel = _connection.CreateModel();

        // Declare the exchange asynchronously
        _channel.ExchangeDeclare(
            exchange: _settings.Exchange,
            type: ExchangeType.Direct);
    }

    public void Dispose()
    {
        // Ensure proper cleanup of async methods
        _channel?.Close();
        _connection?.Close();
    }

    public async Task PublishMessageAsync(string message, string routingKey)
    {
        try
        {
            var body = Encoding.UTF8.GetBytes(message);
            var bodyMemory = new ReadOnlyMemory<byte>(body);

            // Async publish method
            await Task.Run(() =>
            {
                _channel.BasicPublish(
                    exchange: _settings.Exchange,
                    routingKey: routingKey,
                    basicProperties: null, // Default properties, or configure as needed
                    body: bodyMemory);
            });

            Console.WriteLine($"[x] Sent: {message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[!] Error publishing message: {ex.Message}");
        }
    }
}