namespace ImportRation.RabbitMQ;

public interface IMessageService : IDisposable, IRabbitMQPublisher
{
    Task PublishMessageAsync(string message, string routingKey);
}
