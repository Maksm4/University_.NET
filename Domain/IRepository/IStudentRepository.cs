using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.IRepository
{
    public interface IStudentRepository : ICRUDRepository<Student>
    {
        Task<IReadOnlyCollection<Student>> GetAllStudentsWithEnrolledCoursesAsync();
        Task<Student?> GetStudentByUserId(string userId);
        bool IsDetached(Student student);
    }
}