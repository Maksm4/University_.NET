using ApplicationCore.Models;
using ApplicationCore.Models.DTOs;

namespace ApplicationCore.IService
{
    public interface IStudentService
    {
        public Task<IEnumerable<Course>> GetActiveCourses();
        public Task<IEnumerable<Student>> GetAllStudents();
        public Task<IEnumerable<Course>> GetStudentCourses(int studentId);
        public Task<IEnumerable<ModuleMark>> GetStudentMarksFromCourse(int studentId, int courseId);
    }
}