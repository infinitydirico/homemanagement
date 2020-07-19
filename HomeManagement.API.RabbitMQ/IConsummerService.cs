using RabbitMQ.Client.Events;

namespace HomeManagement.API.RabbitMQ
{
    public interface IConsummerService
    {
        string QueueName { get; set; }

        EventingBasicConsumer Consumer { get; }
    }
}
