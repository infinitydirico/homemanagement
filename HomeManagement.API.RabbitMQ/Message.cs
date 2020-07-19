using System.Text;

namespace HomeManagement.API.RabbitMQ
{
    public abstract class Message
    {
        public Message(string queueName)
        {
            QueueName = queueName;
        }

        public string QueueName { get; }

        public abstract string CreateBody();

        internal virtual byte[] EncodeBody() => Encoding.UTF8.GetBytes(CreateBody());

        internal virtual string DecodeMessage() => Encoding.UTF8.GetString(GetBody());

        public abstract byte[] GetBody();
    }
}