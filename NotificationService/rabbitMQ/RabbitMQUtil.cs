using NotificationService.Controllers;
using NotificationService.rabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json.Nodes;

namespace NotificationService.rabbitMQ
{
    public class RabbitMQUtil : IRabbitMQUtil
    {
        private readonly ILogger<RabbitMQUtil> _logger;

        public RabbitMQUtil(ILogger<RabbitMQUtil> logger)
        {
            _logger = logger;
        }

        public async Task PublishMessage(string routingKey,string eventData)
        {

            // creating object
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };

            // create connection
            var connection = factory.CreateConnection();

            // create channel
            using var channel = connection.CreateModel();

            // encoding payload/message
            var body = Encoding.UTF8.GetBytes(eventData);

            channel.BasicPublish("topic.exchange",routingKey,null,body);

            await Task.CompletedTask;
        }

        public async Task ConsumeMessage( IModel channel, string routingKey, CancellationToken cancellationToken)
        {
            // create event consumer
            var consumer = new AsyncEventingBasicConsumer(channel);

            // decoding payload/message
            consumer.Received += async (model, ea ) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body.ToArray());

                // parse response

                var response = JsonObject.Parse(body);
                var requestId = Guid.Parse(response["RequestId"].ToString());
                _logger.LogInformation("Consumed Message from Queue for RequestId "+ requestId);



            };

            channel.BasicConsume("order.notification", true,consumer);

            await Task.CompletedTask;
        }

    }
}
