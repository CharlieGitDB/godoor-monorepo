namespace Identity.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>?> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
    Task<int> SaveAsync(T item);
    Task<int> DeleteAsync(T item);
}