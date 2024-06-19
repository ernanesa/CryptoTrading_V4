using Microsoft.EntityFrameworkCore;
using Trading.Data.PostgreSQL;
using Trading.Entities;

namespace Trading.Repositories
{
    public class UserRepository
    {
        private readonly TradingDbContext _context;

        public UserRepository(TradingDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<List<User>> GetAllActiveAsync()
        {
            return await _context.Users.Where(u => u.IsActive).ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // public async Task<User> GetUserByEmailAsync(string email)
        // {
        //     return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        // }

        // public async Task<User> GetUserByUserNameAsync(string userName)
        // {
        //     return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        // }

        // public async Task<User> GetUserByUserNameOrEmailAsync(string userNameOrEmail)
        // {
        //     return await _context.Users.FirstOrDefaultAsync(u => u.UserName == userNameOrEmail || u.Email == userNameOrEmail);
        // }

        public async Task<User> AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
