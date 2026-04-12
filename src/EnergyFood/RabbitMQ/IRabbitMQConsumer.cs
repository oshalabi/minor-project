namespace EnergyFood.RabbitMQ;

public interface IRabbitMQConsumer
{ 
    Task HandleMessageAsync();
}