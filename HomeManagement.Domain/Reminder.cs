namespace HomeManagement.Domain
{
    public class Reminder
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int DueDay { get; set; }

        public bool Active { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }
    }
}
