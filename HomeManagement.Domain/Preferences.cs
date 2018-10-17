namespace HomeManagement.Domain
{
    public class Preferences
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public string Language { get; set; }
    }
}
