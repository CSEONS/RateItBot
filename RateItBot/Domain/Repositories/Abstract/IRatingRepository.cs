using RateItBot.Domain.Entities;

namespace RateItBot.Domain.Repositories.Abstract
{
    public interface IRatingRepository
    {
        void Add(Rating rating);
    }
}
