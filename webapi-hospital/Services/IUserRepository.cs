using webapi_hospital.Models;

namespace webapi_hospital.Services;

public interface IUserRepository
{
    Task<IEnumerable<User?>> GetAsync();
    Task<User?> GetByIdAsync(int id);
    Task<bool> AddAsync(User user);
    Task<bool> RemoveAsync(int id);
}