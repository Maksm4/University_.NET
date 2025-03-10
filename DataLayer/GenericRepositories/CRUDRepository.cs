using Microsoft.EntityFrameworkCore;

namespace ApplicationCore.GenericRepositories
{
    public class CRUDRepository<T> : ICRUDRepository<T> where T : class
    {
        private readonly DbContext dbContext;

        public CRUDRepository(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task Create(T entity)
        {
            await dbContext.Set<T>().AddAsync(entity);
        }

        public async Task Delete(T entity)
        {
            dbContext.Set<T>().Remove(entity);
        }

        public async Task<IEnumerable<T>> FindAll()
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<T?> FindById(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task Update(T entity)
        {
            dbContext.Set<T>().Update(entity);
        }

        public async Task Save()
        {
            await dbContext.SaveChangesAsync();
        }
    }
} 