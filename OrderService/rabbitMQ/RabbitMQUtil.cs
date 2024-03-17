using RabbitMQ.Client;
using System.Text;

namespace OrderService.rabbitMQ
{
    public class RabbitMQUtil : IRabbitMQUtil
    {
        public async Task PublishMessage(string routingKey,string eventData)
        {
            var rabbitMQHost = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("RabbitMQKeys")["Host"];
            var rabbitMQUser = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("RabbitMQKeys")["User"];
            var rabbitMQPassword = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("RabbitMQKeys")["Password"];

            // creating object
            var factory = new ConnectionFactory
            {
                HostName = rabbitMQHost,
                UserName = rabbitMQUser,
                Password = rabbitMQPassword,
            };

            // create connection
            var connection = factory.CreateConnection();

            // create model for connection
            using var channel = connection.CreateModel();

            // encoding payload/message
            var body = Encoding.UTF8.GetBytes(eventData);

            channel.BasicPublish("topic.exchange",routingKey,null,body);

            await Task.CompletedTask;
        }
        
    }
}
