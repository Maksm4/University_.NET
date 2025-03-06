using ApplicationCore.Models;
using ApplicationCore.Models.DTOs;

namespace ApplicationCore.IService
{
    public interface ICourseService
    {
        public Task<IEnumerable<CourseResponse>> GetActiveCourses();
        public Task<IEnumerable<CourseResponse>> GetStudentCourses(int studentId);
    }
}