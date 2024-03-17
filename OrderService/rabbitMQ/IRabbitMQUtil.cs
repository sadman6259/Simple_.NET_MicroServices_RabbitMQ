namespace OrderService.rabbitMQ
{
    public interface IRabbitMQUtil
    {
        public Task PublishMessage(string routingKey, string eventData);
        

    }
}
