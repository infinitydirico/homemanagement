namespace HomeManagement.Domain
{
    public class ScheduledTransaction
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public TransactionType TransactionType { get; set; }

        public int CategoryId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
