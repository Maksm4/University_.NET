using ApplicationCore.IRepository;
using Domain.Models.Aggregate;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class CRUDRepository<T> : ICRUDRepository<T> where T : class, IAggregateRoot
    {
        private readonly DbContext dbContext;

        public CRUDRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public virtual async Task<T> CreateAsync(T entity)
        {
            var changeTracking = await dbContext.Set<T>().AddAsync(entity);
            return changeTracking.Entity;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public virtual async Task<IReadOnlyCollection<T>> FindAllAsync()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> FindByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }

        public async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}