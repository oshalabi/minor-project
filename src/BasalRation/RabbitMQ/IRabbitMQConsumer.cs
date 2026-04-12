namespace BasalRation.RabbitMQ;

public interface IRabbitMQConsumer
{ 
    Task HandleMessageAsync();
}