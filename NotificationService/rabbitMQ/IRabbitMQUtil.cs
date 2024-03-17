using RabbitMQ.Client;

namespace NotificationService.rabbitMQ
{
    public interface IRabbitMQUtil
    {
        public Task PublishMessage(string routingKey, string eventData);
        public Task ConsumeMessage(IModel channel, string routingKey , CancellationToken cancellationToken);


    }
}
