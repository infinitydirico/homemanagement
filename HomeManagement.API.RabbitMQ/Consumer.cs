using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;

namespace HomeManagement.API.RabbitMQ
{
    public class Consumer
    {
        private readonly IConfiguration configuration;

        public Consumer(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Suscribe(IConsummerService consumerService)
        {
            using (var rabbitConnection = CreateConnectionFactor().CreateConnection())
            using (var channel = rabbitConnection.CreateModel())
            {
                channel.QueueDeclare(consumerService.QueueName, false, false, false, null);

                channel.BasicConsume(
                    queue: consumerService.QueueName,
                    autoAck: true,
                    consumer: consumerService.Consumer);
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
