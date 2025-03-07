using Domain.Models;

namespace ApplicationCore.IService
{
    public interface ICourseService
    {
        public Task<IEnumerable<Course>> GetActiveCourses();
        public Task<IEnumerable<Course>> GetStudentCourses(int studentId);
    }
}