namespace ApplicationCore.GenericRepositories
{
    public interface ICRUDRepository<T>
    {
        Task<IEnumerable<T>> FindAll();
        Task<T?> FindById(int id);
        Task Update(T entity);
        Task Create(T entity);
        Task Delete(T entity);
        Task Save();
    }
}