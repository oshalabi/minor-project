using System.Text;
using Domain.Entities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Ration.DAL;

namespace Ration.RabbitMQ;

public class MessageService : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly BasalRationMessageBroker _settings;
    private readonly IServiceScopeFactory _scopeFactory;

    public MessageService(IOptions<BasalRationMessageBroker> options, IServiceScopeFactory scopeFactory)
    {
        _settings = options.Value;
        _scopeFactory = scopeFactory;

        var factory = new ConnectionFactory
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password,
        };

        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Queue setup
            _channel.QueueDeclare(
                queue: _settings.Queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            // Exchange setup
            _channel.ExchangeDeclare(
                exchange: _settings.Exchange,
                type: ExchangeType.Direct);

            // Queue binding
            _channel.QueueBind(
                queue: _settings.Queue,
                exchange: _settings.Exchange,
                routingKey: _settings.RoutingKey);
            _channel.QueueBind(
                queue: _settings.Queue,
                exchange: _settings.Exchange,
                routingKey: "energyFeed");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to initialize RabbitMQ: {ex.Message}");throw;
        }
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<RationDB>();

            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"[x] Received: {message}");

            try
            {
                var feedType = JsonConvert.DeserializeObject<FeedType>(message);
                await dbContext.FeedTypes.AddAsync(feedType, stoppingToken);
                await dbContext.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        };

        _channel.BasicConsume(
            queue: _settings.Queue,
            autoAck: true,
            consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}
