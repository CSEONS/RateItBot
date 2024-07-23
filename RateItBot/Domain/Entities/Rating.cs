namespace RateItBot.Domain.Entities
{
    public class Rating
    {
        public Guid Id { get; set; }
        public double Value { get; set; }
        public int Score { get; set; }
        public User Owner { get; set; }
        public Guid OwnerId { get; set; }
    }
}
