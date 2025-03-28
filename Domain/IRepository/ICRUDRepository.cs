using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.IRepository
{
    public interface ICRUDRepository<T> where T : class, IAggregateRoot
    {
        Task<IReadOnlyCollection<T>> FindAllAsync();
        Task<T?> FindByIdAsync(int id);
        void Update(T entity);
        Task<T> CreateAsync(T entity);
        void Delete(T entity);
        Task SaveAsync();
    }
}