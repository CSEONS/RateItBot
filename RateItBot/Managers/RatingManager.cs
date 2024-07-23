using RateItBot.Domain.Entities;
using RateItBot.Domain.Repositories.Abstract;

namespace RateItBot.Managers
{
    public class RatingManager
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IUserRepository _userRepository;

        public RatingManager(IRatingRepository ratingRepository, IUserRepository userRepository)
        {
            _ratingRepository = ratingRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<User> GetTop(int topCount)
        {
            return _userRepository.GetAll(true)
                .OrderBy(u => u.Rating
                    .Sum(r => r.Score));
        }

        public void Add(User user, double rateIn, double rateOf)
        {
            var rating = new Rating()
            {
                Owner = user,
                Value = rateIn / rateOf,
                Score = (int)Math.Round((rateIn / rateOf) * 10)
            };

            _ratingRepository.Add(rating);

            user.Rating.Add(rating);

            _userRepository.Update(user);
        }
    }
}
