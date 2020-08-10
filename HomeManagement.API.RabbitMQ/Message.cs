using System.Text;

namespace HomeManagement.API.RabbitMQ
{
    public abstract class Message
    {
        public abstract string QueueName { get; }

        public abstract string CreateBody();

        internal virtual byte[] EncodeBody() => Encoding.UTF8.GetBytes(CreateBody());

        protected virtual string DecodeMessage(byte[] body) => Encoding.UTF8.GetString(body);
    }
}