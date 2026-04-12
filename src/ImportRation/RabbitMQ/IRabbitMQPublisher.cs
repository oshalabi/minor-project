namespace ImportRation.RabbitMQ;

public interface IRabbitMQPublisher
{ 
    Task PublishMessageAsync(string message, string routingKey);
}