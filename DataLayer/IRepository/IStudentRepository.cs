using Domain.Models;

namespace ApplicationCore.IRepository
{
    public interface IStudentRepository : ICRUDRepository<Student>
    {
        Task<IEnumerable<Student>> GetAllStudentsWithLearningPlans();
    }
}