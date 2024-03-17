using NotificationService.rabbitMQ;
using RabbitMQ.Client;

namespace NotificationService.RabbitMQService
{
    public class RabbitMQService : BackgroundService
    {
        private readonly IRabbitMQUtil _rabbitMQUtil;
        private IModel _channel;
        private IConnection _connection;
        public RabbitMQService(IRabbitMQUtil rabbitMQUtil) {

            _rabbitMQUtil =  rabbitMQUtil;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            // creating object
            var factory = new ConnectionFactory
            {
                HostName = "host.docker.internal",
                UserName = "guest",
                Password = "guest",
                DispatchConsumersAsync = true
            };

            // create connection
            _connection = factory.CreateConnection();

            // creating channel
            _channel = _connection.CreateModel();
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync( CancellationToken cancellationToken)
        {
            await _rabbitMQUtil.ConsumeMessage(_channel,"order.notification", cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            _connection.Close();
        }
    }
}
