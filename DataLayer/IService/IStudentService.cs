using Domain.Models;
using Domain.Models.ValueObject;

namespace ApplicationCore.IService
{
    public interface IStudentService
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<IEnumerable<Grade>> GetStudentMarksFromCourse(int studentId, int courseId);
        Task<Student> AddLearningPlan(int studentId, LearningPlan learningPlan);
        Task<IEnumerable<IndividualCourse>> GetStudentCourses(int studentId);
    }
}