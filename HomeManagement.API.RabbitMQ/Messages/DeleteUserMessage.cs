using System;
using System.Linq;

namespace HomeManagement.API.RabbitMQ.Messages
{
    public class DeleteUserMessage : Message
    {
        public DeleteUserMessage() { }
        public DeleteUserMessage(byte[] array)
        {
            var value = DecodeMessage(array);
            var values = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Email = values.First().Split(':').Last();
        }


        public override string QueueName => "DeleteUserQueue";

        public string Email { get; set; }

        public override string CreateBody() => $"Email:{Email}";
    }
}
