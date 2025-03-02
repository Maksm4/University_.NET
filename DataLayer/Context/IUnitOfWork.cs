using dataLayer.Context;
using DataLayer.IRepository;

namespace DataLayer.Context
{
    public interface IUnitOfWork
    {
        IStudentRepository StudentRepository { get; }
        UniversityContext UniversityContext { get; }
        Task SaveAsync();
    }
}