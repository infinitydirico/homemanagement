using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace HomeManagement.API.RabbitMQ
{
    public abstract class Consumer
    {
        protected readonly IServiceProvider serviceProvider;

        public Consumer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public abstract string QueueName { get; }

        public abstract EventingBasicConsumer CreateConsumer(IModel channel);

        public void Suscribe()
        {
            using (var rabbitConnection = CreateConnectionFactor().CreateConnection())
            using (var channel = rabbitConnection.CreateModel())
            {
                channel.QueueDeclare(QueueName, false, false, false, null);

                channel.BasicConsume(
                    queue: QueueName,
                    autoAck: true,
                    consumer: CreateConsumer(channel));
            }
        }

        private ConnectionFactory CreateConnectionFactor()
        {
            var configuration = serviceProvider.GetService(typeof(IConfiguration)) as IConfiguration;
            var connection = new ConnectionFactory
            {
                HostName = configuration.GetSection("RabbitMQ:HostName").Value ?? throw new ArgumentNullException("Configuration RabbitMQ:HostName cannot be null."),
                UserName = configuration.GetSection("RabbitMQ:UserName").Value ?? throw new ArgumentNullException("Configuration RabbitMQ:UserName cannot be null."),
                Password = configuration.GetSection("RabbitMQ:Password").Value ?? throw new ArgumentNullException("Configuration RabbitMQ:Password cannot be null."),
                Port = int.Parse(configuration.GetSection("RabbitMQ:Port").Value),
                RequestedConnectionTimeout = TimeSpan.FromSeconds(int.Parse(configuration.GetSection("RabbitMQ:RequestedConnectionTimeout").Value))
            };

            return connection;
        }
    }
}
