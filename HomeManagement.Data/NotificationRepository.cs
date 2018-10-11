using HomeManagement.Domain;

namespace HomeManagement.Data
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(Notification entity) => GetById(entity.Id) != null;

        public override Notification GetById(int id) => FirstOrDefault(x => x.Id.Equals(id));
    }
}
