using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.IService
{
    public interface IStudentReadService
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<IEnumerable<Course>> GetCoursesTakenByStudent(int studentId);
        Task<IEnumerable<MarkedModule>> GetStudentMarksForCourse(int studentId, int courseId);
    }
}