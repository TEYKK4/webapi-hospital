using Microsoft.EntityFrameworkCore;
using webapi_hospital.Models;

namespace webapi_hospital.Services;

public class UserRepository : IUserRepository
{
    private readonly HospitalDbContext _dbContext;

    public UserRepository(HospitalDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<User?>> GetAsync()
    {
        return await _dbContext.Users.ToArrayAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _dbContext.Users.SingleOrDefaultAsync(u => u != null && u.Id == id);
    }

    public async Task<bool> AddAsync(User ser)
    {
        try
        {
            await _dbContext.AddAsync(ser);
            return await _dbContext.SaveChangesAsync() >= 1;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> RemoveAsync(int id)
    {
        try
        {
            var user = await GetByIdAsync(id);
            _dbContext.Remove(user);
            return await _dbContext.SaveChangesAsync() >= 1;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}