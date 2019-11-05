using HomeManagement.Domain;
using System.Collections.Generic;
using System.Linq;

namespace HomeManagement.Data
{
    public class ReminderRepository : BaseRepository<Reminder>, IReminderRepository
    {
        public ReminderRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(Reminder entity) => GetById(entity.Id) != null;

        public override Reminder GetById(int id) => FirstOrDefault(x => x.Id.Equals(id));

        public IEnumerable<Reminder> GetByUser(string username)
        {
            var reminderSet = platformContext.GetDbContext().Set<Reminder>().AsQueryable();

            var reminders = from reminder in reminderSet
                            where reminder.User.Email.Equals(username)
                            select reminder;

            return reminders.ToList();
        }

        public IEnumerable<Reminder> GetUserActiveReminders(string username)
        {
            var reminderSet = platformContext.GetDbContext().Set<Reminder>().AsQueryable();

            var reminders = from reminder in reminderSet
                            where reminder.User.Email.Equals(username) && reminder.Active
                            select reminder;

            return reminders.ToList();
        }
    }
}
