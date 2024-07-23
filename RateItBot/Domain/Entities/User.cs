namespace RateItBot.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public long TelegramId { get; set; }
        public string UserName { get; set; }
        public ICollection<Rating> Rating { get; set; }
    }
}
