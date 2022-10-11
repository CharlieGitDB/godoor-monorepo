using Identity.Domain.Entities;

namespace Identity.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByOidAsync(string oid);
}