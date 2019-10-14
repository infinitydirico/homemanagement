using HomeManagement.Domain;

namespace HomeManagement.Data
{
    public class ScheduledTransactionRepository : BaseRepository<ScheduledTransaction>, IScheduledTransactionRepository
    {
        public ScheduledTransactionRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(ScheduledTransaction entity)
            => GetById(entity.Id) != null;

        public override ScheduledTransaction GetById(int id)
            => FirstOrDefault(x => x.Id.Equals(id));
    }
}
