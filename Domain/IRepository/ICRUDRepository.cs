using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.IRepository
{
    public interface ICRUDRepository<T> where T : class, IAggregateRoot
    {
        Task<IEnumerable<T>> FindAll();
        Task<T?> FindById(int id);
        Task Update(T entity);
        Task Create(T entity);
        Task Delete(T entity);
        Task Save();
    }
}