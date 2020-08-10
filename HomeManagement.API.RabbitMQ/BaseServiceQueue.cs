using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeManagement.API.RabbitMQ
{
    public abstract class BaseServiceQueue : BackgroundService
    {
        protected readonly IServiceScopeFactory serviceScopeFactory;
        protected readonly IConfiguration configuration;
        protected IModel _channel;
        protected IConnection _connection;

        public BaseServiceQueue(IServiceScopeFactory serviceScopeFactory,
            IConfiguration configuration)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.configuration = configuration;
        }

        public abstract string QueueName { get; }

        private async Task Connect()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = configuration.GetSection("RabbitMQ:HostName").Value ?? throw new ArgumentNullException("Configuration RabbitMQ:HostName cannot be null."),
                    UserName = configuration.GetSection("RabbitMQ:UserName").Value ?? throw new ArgumentNullException("Configuration RabbitMQ:UserName cannot be null."),
                    Password = configuration.GetSection("RabbitMQ:Password").Value ?? throw new ArgumentNullException("Configuration RabbitMQ:Password cannot be null."),
                    Port = int.Parse(configuration.GetSection("RabbitMQ:Port").Value),
                    RequestedConnectionTimeout = TimeSpan.FromSeconds(int.Parse(configuration.GetSection("RabbitMQ:RequestedConnectionTimeout").Value))
                };

                _connection = factory.CreateConnection();
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: QueueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
                _channel.BasicQos(0, 1, false);

                await Consume();
            }
            catch (Exception ex)
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
                    logger.LogError(ex, "Fail to connect to rabbit mq service.");
                }
                await Task.Delay(1000 * 60 * 10);
                await Connect();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            await Connect();            
        }

        private async Task Consume()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += OnRecieved;

            consumer.Shutdown += OnConsumerShutdown;
            consumer.Registered += OnConsumerRegistered;
            consumer.Unregistered += OnConsumerUnregistered;
            consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

            _channel.BasicConsume(QueueName, true, consumer);

            await Task.Yield();
        }

        protected void OnMessageRecieved(object sender, BasicDeliverEventArgs ea)
        {
            OnRecieved(sender, ea);
            _channel.BasicAck(ea.DeliveryTag, false);
        }

        protected abstract void OnRecieved(object sender, BasicDeliverEventArgs ea);

        protected abstract void OnConsumerShutdown(object sender, ShutdownEventArgs e);

        protected abstract void OnConsumerRegistered(object sender, ConsumerEventArgs e);

        protected abstract void OnConsumerUnregistered(object sender, ConsumerEventArgs e);

        protected abstract void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e);

        protected abstract void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e);

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
