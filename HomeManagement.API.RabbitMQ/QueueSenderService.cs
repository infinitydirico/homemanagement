using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace HomeManagement.API.RabbitMQ
{
    public interface IQueueService
    {
        void SendMessage(Message message);
    }

    public class QueueSenderService : IQueueService
    {
        private readonly IConfiguration configuration;

        public QueueSenderService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendMessage(Message message)
        {
            using(var rabbitConnection = CreateConnectionFactor().CreateConnection())
            using(var channel = rabbitConnection.CreateModel())
            {
                channel.QueueDeclare(message.QueueName, true, false, false, null);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: message.QueueName,
                    basicProperties: properties,
                    body: message.EncodeBody());
            }
        }

        private ConnectionFactory CreateConnectionFactor() => new ConnectionFactory
        {
            HostName = configuration.GetSection("RabbitMQ:HostName").Value ?? throw new ArgumentNullException("Configuration RabbitMQ:HostName cannot be null."),
            UserName = configuration.GetSection("RabbitMQ:UserName").Value ?? throw new ArgumentNullException("Configuration RabbitMQ:UserName cannot be null."),
            Password = configuration.GetSection("RabbitMQ:Password").Value ?? throw new ArgumentNullException("Configuration RabbitMQ:Password cannot be null."),
            Port = int.Parse(configuration.GetSection("RabbitMQ:Port").Value),
            RequestedConnectionTimeout = TimeSpan.FromSeconds(int.Parse(configuration.GetSection("RabbitMQ:RequestedConnectionTimeout").Value))
        };
    }
}
