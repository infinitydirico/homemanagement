using Grpc.Core;
using HomeManagement.Api.Bus.Protos;
using System.Linq;
using System.Threading.Tasks;

namespace HomeManagement.Api.Bus.Services
{
    public class ListenerService : Protos.ListenerService.ListenerServiceBase
    {
        private readonly IQueueService queueService;

        public ListenerService(IQueueService queueService)
        {
            this.queueService = queueService;
        }

        public override async Task<EventReply> Check(ListenerMessage request, ServerCallContext context)
        {
            var queues = queueService.GetQueues(request.MessageType);

            var reply = new EventReply
            {
                Result = queues.Any()
            };

            return await Task.FromResult(reply);
        }

        public override async Task<RegisterUsers> GetRegistrations(RegistrationRequest request, ServerCallContext context)
        {
            var queues = queueService.GetQueues(EventType.Registration);

            var reply = new RegisterUsers();
            reply.Emails.AddRange(queues.Select(x => x.Value).ToList());

            queueService.Clear(queues);

            return await Task.FromResult(reply);
        }

        public override async Task<EventReply> PushNewRegistration(RegistrationSender request, ServerCallContext context)
        {
            queueService.Add(new Queue
            {
                Type = EventType.Registration,
                Value = request.Email
            });

            return await Task.FromResult(new EventReply());
        }
    }
}