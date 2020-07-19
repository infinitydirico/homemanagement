using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace HomeManagement.API.RabbitMQ
{
    public interface IConsummerService
    {
        string QueueName { get; }

        EventingBasicConsumer Consumer { get; }

        void BindChannel(IModel channel);
    }
}
