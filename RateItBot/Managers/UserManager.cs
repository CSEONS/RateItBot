using RateItBot.Domain.Entities;
using RateItBot.Domain.Repositories.Abstract;

namespace RateItBot.Managers
{
    public class UserManager
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Create(long telegrmaId, string username)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                TelegramId = telegrmaId,
                UserName = username,
                Rating = []
            };

            _userRepository.Add(user);

            return user;
        }

        public bool Exist(User user)
        {
            return _userRepository.Exist(user);
        }

        public bool ExistByTelegramId(long telegrmaId)
        {
            return _userRepository.ExistByTelegramId(telegrmaId);
        }
    }
}
