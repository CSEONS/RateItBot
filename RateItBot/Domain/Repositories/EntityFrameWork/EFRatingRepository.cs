using RateItBot.Domain.Entities;
using RateItBot.Domain.Repositories.Abstract;

namespace RateItBot.Domain.Repositories.EntityFrameWork
{
    public class EFRatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _context;

        public EFRatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Rating rating)
        {
            _context.Rating.Add(rating);
            _context.SaveChanges();
        }
    }
}
