using RateItBot.Domain.Entities;

namespace RateItBot.Domain.Repositories.Abstract
{
    public interface IUserRepository
    {
        void Add(User user);
        User GetById(Guid id);
        User GetByUserName(string userName);
        User GetByTelegramId(long id);
        void Update(User user);
        bool Exist(User user);
        bool ExistByTelegramId(long id);
        IEnumerable<User> GetAll(bool eager=false);
    }
}
