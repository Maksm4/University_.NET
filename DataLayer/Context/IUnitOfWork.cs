namespace DataLayer.Context
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}