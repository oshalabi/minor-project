using ImportRation.RabbitMQ;

namespace ImportRation.Tests;

public class FakeMessageService : IMessageService
{
    public Task PublishMessageAsync(string message, string routingKey)
    {
        
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        // Dispose of any resources if necessary (none in this fake implementation)
        // Simply providing an empty implementation
    }
}