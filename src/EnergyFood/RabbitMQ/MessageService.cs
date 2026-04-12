using System.Text;
using Domain.Entities;
using EnergyFood.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using IModel = RabbitMQ.Client.IModel;

namespace EnergyFood.RabbitMQ;

public class MessageService : IDisposable, IRabbitMQConsumer
{
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly BasalRationMessageBroker _settings;
    private readonly IServiceScopeFactory _scopeFactory;


    public MessageService(IOptions<BasalRationMessageBroker> options, IServiceScopeFactory scopeFactory)
    {
        _settings = options.Value;
        _scopeFactory = scopeFactory;

        var factory = new ConnectionFactory()
        {
            HostName = _settings.Host,
            Port = _settings.Port,
            UserName = _settings.Username,
            Password = _settings.Password
        };


        try
        {
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _settings.Queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            _channel.ExchangeDeclare(
                exchange: _settings.Exchange,
                type: ExchangeType.Direct);

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
            Console.WriteLine($"Failed to initialize RabbitMQ: {ex.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }

public Task HandleMessageAsync()
{
    var consumer = new EventingBasicConsumer(_channel);

    consumer.Received += async (model, ea) =>
    {
        using var scope = _scopeFactory.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EnergyFoodDB>();

        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($"[x] Received: {message}");

        var feedType = JsonConvert.DeserializeObject<FeedType>(message);

        foreach (var nutrient in feedType.Nutrients)
        {
            var existingNutrientType = await dbContext.NutrientTypes
                .AsTracking()
                .FirstOrDefaultAsync(nt => nt.Value == nutrient.NutrientType.Value);

            if (existingNutrientType == null)
            {
                var newNutrientType = new NutrientType
                {
                    Code = nutrient.NutrientType.Code,
                    Value = nutrient.NutrientType.Value
                };

                await dbContext.NutrientTypes.AddAsync(newNutrientType);
                await dbContext.SaveChangesAsync();

                nutrient.NutrientTypeId = newNutrientType.Id;
                nutrient.NutrientType = newNutrientType;
            }
            else
            {
                nutrient.NutrientTypeId = existingNutrientType.Id;
                nutrient.NutrientType = existingNutrientType;
            }
        }

        await dbContext.FeedTypes.AddAsync(feedType);
        await dbContext.SaveChangesAsync();
    };

    _channel.BasicConsume(
        queue: _settings.Queue,
        autoAck: true,
        consumer: consumer);

    return Task.CompletedTask;
}
}
