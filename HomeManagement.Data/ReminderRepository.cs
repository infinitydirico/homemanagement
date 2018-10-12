using HomeManagement.Domain;

namespace HomeManagement.Data
{
    public class ReminderRepository : BaseRepository<Reminder>, IReminderRepository
    {
        public ReminderRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(Reminder entity) => GetById(entity.Id) != null;

        public override Reminder GetById(int id) => FirstOrDefault(x => x.Id.Equals(id));
    }
}
