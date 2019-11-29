using HomeManagement.Api.Bus.Protos;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Api.Bus.Services
{
    public class QueueService : IQueueService
    {
        private readonly List<Queue> queues = new List<Queue>();

        public void Add(Queue queue)
        {
            queues.Add(queue);
        }

        public void Clear(IEnumerable<Queue> qs)
        {
            foreach (var q in qs)
            {
                var index = queues.IndexOf(q);
                queues.RemoveAt(index);
            }
        }

        public IEnumerable<Queue> GetQueues() => queues.ToList();

        public IEnumerable<Queue> GetQueues(EventType eventType) => GetQueues().Where(x => x.Type.Equals(eventType));
    }

    public interface IQueueService
    {
        void Add(Queue queue);

        void Clear(IEnumerable<Queue> queues);

        IEnumerable<Queue> GetQueues();

        IEnumerable<Queue> GetQueues(EventType eventType);
    }

    public class Queue
    {
        public EventType Type { get; set; }

        public string Value { get; set; }
    }
}