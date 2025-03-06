namespace ApplicationCore.Context
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
    }
}