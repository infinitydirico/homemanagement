namespace HomeManagement.Models
{
    public class NotificationModel
    {
        public int Id { get; set; }

        public int ReminderId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int DueDay { get; set; }

        public bool Dismissed { get; set; }
    }
}
