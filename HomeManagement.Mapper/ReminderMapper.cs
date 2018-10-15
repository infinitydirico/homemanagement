using HomeManagement.Contracts.Mapper;
using HomeManagement.Domain;
using HomeManagement.Models;
using System.Collections.Generic;
using System.Reflection;

namespace HomeManagement.Mapper
{
    public interface IReminderMapper : IMapper<Reminder, ReminderModel>
    {
    }

    public class ReminderMapper : BaseMapper<Reminder, ReminderModel>, IReminderMapper
    {
        public override IEnumerable<PropertyInfo> GetEntityProperties()
        {
            yield return typeof(Reminder).GetProperty(nameof(Reminder.Id));
            yield return typeof(Reminder).GetProperty(nameof(Reminder.Title));
            yield return typeof(Reminder).GetProperty(nameof(Reminder.Description));
            yield return typeof(Reminder).GetProperty(nameof(Reminder.DueDay));
            yield return typeof(Reminder).GetProperty(nameof(Reminder.Active));
        }

        public override IEnumerable<PropertyInfo> GetModelProperties()
        {
            yield return typeof(ReminderModel).GetProperty(nameof(ReminderModel.Id));
            yield return typeof(ReminderModel).GetProperty(nameof(ReminderModel.Title));
            yield return typeof(ReminderModel).GetProperty(nameof(ReminderModel.Description));
            yield return typeof(ReminderModel).GetProperty(nameof(ReminderModel.DueDay));
            yield return typeof(ReminderModel).GetProperty(nameof(ReminderModel.Active));
        }
    }
}
