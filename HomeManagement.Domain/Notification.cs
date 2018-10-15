using System;

namespace HomeManagement.Domain
{
    public class Notification
    {
        public int Id { get; set; }

        public Reminder Reminder { get; set; }

        public int ReminderId { get; set; }

        public bool Dismissed { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
