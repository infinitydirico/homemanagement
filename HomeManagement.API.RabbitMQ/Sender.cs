using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace HomeManagement.API.RabbitMQ
{
    public class Sender
    {
        private readonly IConfiguration configuration;

        public Sender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void SendMessage(Message message)
        {
            using(var rabbitConnection = CreateConnectionFactor().CreateConnection())
            using(var channel = rabbitConnection.CreateModel())
            {
                channel.QueueDeclare(message.QueueName, false, false, false, null);
                channel.BasicPublish(
                    exchange: string.Empty,
                    routingKey: message.QueueName,
                    basicProperties: null,
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
