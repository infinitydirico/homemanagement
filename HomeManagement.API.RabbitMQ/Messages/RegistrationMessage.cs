using HomeManagement.API.RabbitMQ;
using System;
using System.Linq;

namespace HomeManagement.API.Queue.Messages
{
    public class RegistrationMessage : Message
    {
        public RegistrationMessage() { }
        public RegistrationMessage(byte[] array)
        {
            var value = DecodeMessage(array);
            var values = value.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            Email = values.First().Split(':').Last();
            Language = values.Last().Split(':').Last();
        }

        public string Email { get; set; }

        public string Language { get; set; }

        public override string QueueName => "RegistrationQueue";

        public override string CreateBody()
        {
            return $"{nameof(Email)}:{Email}" +
                $"{Environment.NewLine}" +
                $"{nameof(Language)}:{Language}";
        }
    }
}
