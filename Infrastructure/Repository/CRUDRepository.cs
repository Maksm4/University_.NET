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
        public virtual async Task Create(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
        }

        public virtual async Task Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public virtual async Task<IEnumerable<T>> FindAll()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public virtual async Task<T?> FindById(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}