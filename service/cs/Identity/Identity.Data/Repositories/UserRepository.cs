using Identity.Domain.Entities;
using Identity.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data.Services;

public class UserRepository : IGenericRepository<User>
{
    public IdentityDbContext _dbContext { get; set; }

    public UserRepository(IdentityDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _dbContext.Users.FindAsync(id);
    }

    public async Task<int> SaveAsync(User user)
    {
        _dbContext.Users.Add(user);
        return await _dbContext.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(User user)
    {
        _dbContext.Users.Remove(user);
        return await _dbContext.SaveChangesAsync();
    }
}