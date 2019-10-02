namespace HomeManagement.Domain
{
    public class Preferences
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
