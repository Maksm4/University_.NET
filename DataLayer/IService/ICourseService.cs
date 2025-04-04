using Domain.Models;
using Domain.Models.Aggregate;

namespace ApplicationCore.IService
{
    public interface ICourseService
    {
        Task<IReadOnlyCollection<Course>> GetCoursesAsync();
        Task<Course?> GetCourseAsync(int courseId);
        Task<IReadOnlyCollection<CourseModule>> GetCourseModules(int courseId);
    }
}