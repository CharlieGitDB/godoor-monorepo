using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data.Repositories;

public class UserRepository : IUserRepository
{
    public IdentityDbContext _dbContext { get; set; }

    public UserRepository(IdentityDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<User>?> GetAllAsync() => await _dbContext.Users.ToListAsync();

    public async Task<User?> GetByIdAsync(string oid) => await _dbContext.Users.FindAsync(oid);

    public async Task<int> SaveAsync(User user)
    {
        if (await _dbContext.Users.FindAsync(user.Oid) is User existing)
        {
            _dbContext.Entry(existing).CurrentValues.SetValues(user);
        }
        else
        {
            _dbContext.Users.Add(user);
        }

        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);
        return await _dbContext.SaveChangesAsync();
    }
}