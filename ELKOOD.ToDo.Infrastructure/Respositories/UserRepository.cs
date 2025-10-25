using Microsoft.EntityFrameworkCore;
using ELKOOD.ToDo.Core.Entities;
using ELKOOD.ToDo.Core.Interfaces;
using ELKOOD.ToDo.Infrastructure.Data;

namespace ELKOOD.ToDo.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<bool> UserExistsAsync(string email, string username)
        {
            return await _dbSet
                .AnyAsync(u => u.Email == email || u.Username == username);
        }
    }
}