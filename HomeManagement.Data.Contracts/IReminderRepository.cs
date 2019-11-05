using HomeManagement.Contracts.Repositories;
using HomeManagement.Domain;
using System.Collections.Generic;

namespace HomeManagement.Data
{
    public interface IReminderRepository : IRepository<Reminder>
    {
        IEnumerable<Reminder> GetByUser(string username);

        IEnumerable<Reminder> GetUserActiveReminders(string username);
    }
}
