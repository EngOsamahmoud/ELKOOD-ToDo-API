using ELKOOD.ToDo.Core.Entities;

namespace ELKOOD.ToDo.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> UserExistsAsync(string email, string username);
    }
}