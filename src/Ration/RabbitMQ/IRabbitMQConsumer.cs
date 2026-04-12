namespace Ration.RabbitMQ;

public interface IRabbitMQConsumer
{ 
    Task HandleMessageAsync();
}