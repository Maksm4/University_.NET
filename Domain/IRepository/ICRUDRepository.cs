using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.IRepository
{
    public interface ICRUDRepository<T> where T : class, IAggregateRoot
    {
        Task<IReadOnlyCollection<T>> FindAllAsync();
        Task<T?> FindByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task SaveAsync();
    }
}