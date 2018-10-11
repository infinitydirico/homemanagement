namespace HomeManagement.Domain
{
    public class Notification
    {
        public int Id { get; set; }

        public int ReminderId { get; set; }

        public bool Dismissed { get; set; }
    }
}
