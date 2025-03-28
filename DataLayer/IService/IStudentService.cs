using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.IService
{
    public interface IStudentService
    {
        Task<IReadOnlyCollection<Student>> GetAllStudentsAsync();
        Task<IReadOnlyCollection<Course>> GetCoursesTakenByStudentAsync(int studentId);
        Task<IReadOnlyCollection<MarkedModule>> GetStudentMarksForCourseAsync(int studentId, int courseId);
        Task<Student?> GetStudent(int studentId);
        Task<Student> SaveStudent(Student student);
        Task<Student?> GetStudentByUserId(string userId);
    }
}