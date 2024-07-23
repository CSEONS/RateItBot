using Microsoft.EntityFrameworkCore;
using RateItBot.Domain.Entities;
using RateItBot.Domain.Repositories.Abstract;

namespace RateItBot.Domain.Repositories.EntityFrameWork
{
    public class EFUserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public EFUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public bool Exist(User user)
        {
            return _context.Users.Any(u => u.Id == user.Id);
        }

        public bool ExistByTelegramId(long id)
        {
            return _context.Users.Any(u => u.TelegramId == id);
        }

        public IEnumerable<User> GetAll(bool eager = false)
        {
            if (eager)
            {
                return _context.Users.Include(u => u.Rating);
            }
            else
            {
                return _context.Users;
            }
        }

        public User GetById(Guid id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetByTelegramId(long telegramId)
        {
            return _context.Users.Include(u => u.Rating).FirstOrDefault(u => u.TelegramId == telegramId);
        }

        public User GetByUserName(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.UserName ==  userName);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges(true);
        }
    }
}
