using Domain.Models.Aggregate;

namespace ApplicationCore.IRepository
{
    public interface IStudentRepository : ICRUDRepository<Student>
    {
        Task<IEnumerable<Student>> GetAllStudentsWithEnrolledCourses();
    }
}