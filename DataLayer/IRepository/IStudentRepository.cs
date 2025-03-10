using ApplicationCore.GenericRepositories;
using Domain.Models;

namespace ApplicationCore.IRepository
{
    public interface IStudentRepository : ICRUDRepository<Student>
    {
        Task<Student?> GetStudentInfo(int studentId);
        Task<IEnumerable<Student>> GetAllStudentsWithLearningPlans();
    }
}